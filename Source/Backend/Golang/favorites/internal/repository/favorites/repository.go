package favorites

import (
	"context"
	"fmt"
	"strings"

	"github.com/Masterminds/squirrel"
	"github.com/Reswero/Marketplace-v1/favorites/internal/domain/product"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
)

const (
	favoritesTable = "favorites"
)

// Репозиторий избранного
type Repository struct {
	*postgres.Storage
}

func New(s *postgres.Storage) *Repository {
	return &Repository{Storage: s}
}

// Добавить товар в избранное покупателя
func (r *Repository) AddProduct(ctx context.Context, fav *product.FavoriteProduct) error {
	const op = "repository.favorites.AddProduct"

	sql, args, err := r.Builder.Insert(favoritesTable).
		Columns("customer_id", "product_id").
		Values(fav.CustomerId, fav.ProductId).
		ToSql()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Получить список избранных товаров покупателя
func (r *Repository) GetProductList(ctx context.Context, customerId, offset, limit int) ([]*product.FavoriteProduct, error) {
	const op = "repository.favorites.GetProductList"

	sql, args, err := r.Builder.Select("customer_id", "product_id").
		From(favoritesTable).
		Where(squirrel.Eq{"customer_id": customerId}).
		Offset(uint64(offset)).
		Limit(uint64(limit)).
		ToSql()
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	rows, err := r.Pool.Query(ctx, sql, args...)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}
	defer rows.Close()

	favs := make([]*product.FavoriteProduct, 0)
	for rows.Next() {
		fav := &product.FavoriteProduct{}

		err = rows.Scan(&fav.CustomerId, &fav.ProductId)
		if err != nil {
			return nil, formatter.FmtError(op, err)
		}

		favs = append(favs, fav)
	}

	return favs, nil
}

// Удалить товар из избранного покупателя
func (r *Repository) DeleteProduct(ctx context.Context, fav *product.FavoriteProduct) error {
	const op = "repository.favorites.DeleteProduct"

	sql, args, err := r.Builder.Delete(favoritesTable).
		Where(squirrel.Eq{"customer_id": fav.CustomerId, "product_id": fav.ProductId}).
		ToSql()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Проверить находятся ли товары в избранном у покупателя
func (r *Repository) CheckProductsInFavorites(ctx context.Context, customerId int, productIds []int) ([]int, error) {
	const op = "repository.favorites.CheckProductsInFavorites"

	placeholders := make([]string, len(productIds))
	for i := range placeholders {
		placeholders[i] = fmt.Sprintf("$%d", i+2)
	}

	sql, args, err := r.Builder.Select("product_id").
		From(favoritesTable).
		Where(squirrel.Eq{"customer_id": customerId}).
		Where(squirrel.Expr("product_id IN (" + strings.Join(placeholders, ",") + ")")).
		ToSql()
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	for _, id := range productIds {
		args = append(args, id)
	}

	rows, err := r.Pool.Query(ctx, sql, args...)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}
	defer rows.Close()

	inFavorite := make([]int, 0)
	for rows.Next() {
		var productId int

		err = rows.Scan(&productId)
		if err != nil {
			return nil, formatter.FmtError(op, err)
		}

		inFavorite = append(inFavorite, productId)
	}

	return inFavorite, nil
}
