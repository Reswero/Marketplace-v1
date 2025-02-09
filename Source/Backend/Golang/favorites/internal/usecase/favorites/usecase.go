package favorites

import (
	"context"

	"github.com/Reswero/Marketplace-v1/favorites/internal/domain/product"
	"github.com/Reswero/Marketplace-v1/favorites/internal/usecase"
	"github.com/Reswero/Marketplace-v1/favorites/internal/usecase/adapters/repository"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/products"
)

// Взаимодействие с избранными товарами покупателей
type UseCase struct {
	repo     repository.Favorites
	products *products.Service
}

func New(r repository.Favorites, p *products.Service) *UseCase {
	return &UseCase{
		repo:     r,
		products: p,
	}
}

// Добавить товар в избранное покупателя
func (u *UseCase) AddProduct(ctx context.Context, fav *usecase.FavoriteProductDto) error {
	const op = "usecase.favorites.AddProduct"

	existingIds, err := u.products.CheckProductsExistence(ctx, []int{fav.ProductId})
	if err != nil {
		return formatter.FmtError(op, err)
	}

	if len(existingIds) < 1 {
		return usecase.ErrProductNotExists
	}

	dFav := &product.FavoriteProduct{
		CustomerId: fav.CustomerId,
		ProductId:  fav.ProductId,
	}

	err = u.repo.AddProduct(ctx, dFav)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Получить список избранных товаров покупателя
func (u *UseCase) GetProductList(ctx context.Context, customerId int,
	pagination *usecase.PaginationDto) ([]*product.FavoriteProduct, error) {
	const op = "usecase.favorites.GetProductList"

	favs, err := u.repo.GetProductList(ctx, customerId, pagination.Offset, pagination.Limit)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return favs, nil
}

// Удалить товар из избранного покупателя
func (u *UseCase) DeleteProduct(ctx context.Context, fav *usecase.FavoriteProductDto) error {
	const op = "usecase.favorites.DeleteProduct"

	dFav := &product.FavoriteProduct{
		CustomerId: fav.CustomerId,
		ProductId:  fav.ProductId,
	}

	err := u.repo.DeleteProduct(ctx, dFav)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Проверить находятся ли товары в избранном у покупателя
func (u *UseCase) CheckProductsInFavorites(ctx context.Context, customerId int, productIds []int) ([]int, error) {
	const op = "usecase.favorites.CheckProductsInFavorite"

	ids, err := u.repo.CheckProductsInFavorites(ctx, customerId, productIds)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return ids, nil
}
