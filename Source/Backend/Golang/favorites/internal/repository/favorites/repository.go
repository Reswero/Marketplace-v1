package favorites

import (
	"context"

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

	rows, err := r.Pool.Query(ctx, sql, args)
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
