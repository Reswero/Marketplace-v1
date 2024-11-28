package cart

import (
	"context"

	domain "github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

// Сервис кэша для корзины
type Cache struct {
	*redis.Cache
}

func New(r *redis.Cache) *Cache {
	return &Cache{
		Cache: r,
	}
}

func (c *Cache) AddProduct(ctx context.Context, customerId, productId int) error {
	return nil
}

func (c *Cache) GetProducts(ctx context.Context, customerId int) ([]domain.Product, error) {
	return nil, nil
}

func (c *Cache) DeleteProduct(ctx context.Context, customerId, productId int) error {
	return nil
}

func (c *Cache) ChangeProductCount(ctx context.Context, customerId, productId, count int) error {
	return nil
}

func (c *Cache) Clear(ctx context.Context, customerId int) error {
	return nil
}
