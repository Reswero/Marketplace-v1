package usecase

import (
	"context"

	domain "github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
)

// Взаимодействие с корзиной
type Cart interface {
	// Добавить товар в корзину покупателя
	AddProduct(ctx context.Context, customerId, productId int) error
	// Получить товары из корзины покупателя
	GetProducts(ctx context.Context, customerId int) ([]*domain.Product, error)
	// Удалить товар из корзины покупателя
	DeleteProduct(ctx context.Context, customerId, productId int) error
	// Изменить количество товара в корзине покупателя
	ChangeProductCount(ctx context.Context, customerId, productId, countToAdd int) error
	// Очистить корзину покупателя
	Clear(ctx context.Context, customerId int) error
}
