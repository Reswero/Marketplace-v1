package repository

import (
	"context"

	"github.com/Reswero/Marketplace-v1/favorites/internal/domain/product"
)

// Репозиторий избранного
type Favorites interface {
	// Добавить товар в избранное покупателя
	AddProduct(ctx context.Context, fav *product.FavoriteProduct) error
	// Получить список избранных товаров покупателя
	GetProductList(ctx context.Context, customerId, offset, limit int) ([]*product.FavoriteProduct, error)
	// Удалить товар из избранного покупател
	DeleteProduct(ctx context.Context, fav *product.FavoriteProduct) error
}
