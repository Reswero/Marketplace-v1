package usecase

import (
	"context"

	"github.com/Reswero/Marketplace-v1/favorites/internal/domain/product"
)

// Взаимодействие с избранными товарами покупателей
type Favorites interface {
	// Добавить товар в избранное покупателя
	AddProduct(ctx context.Context, fav *FavoriteProductDto) error
	// Получить список избранных товаров покупателя
	GetProductList(ctx context.Context, customerId int, pagination *PaginationDto) ([]*product.FavoriteProduct, error)
	// Удалить товар из избранного покупателя
	DeleteProduct(ctx context.Context, fav *FavoriteProductDto) error
}
