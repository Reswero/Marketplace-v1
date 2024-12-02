package cart

import (
	"context"
	"encoding/json"
	"fmt"
	"time"

	domain "github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

const (
	cartKey      = "cart:%d"
	cartLiveTime = time.Hour * 24 * 7
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

// Добавить товар в корзину покупателя
func (c *Cache) AddProduct(ctx context.Context, customerId, productId int) error {
	const op = "cache.cart.AddProduct"

	key := getCartKey(customerId)

	product := NewProduct(customerId, productId)
	val, err := json.Marshal(product)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = c.Client.LPush(ctx, key, val).Err()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = c.updateCartExpirationTime(ctx, key)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Получить товары из корзины покупателя
func (c *Cache) GetProducts(ctx context.Context, customerId int) ([]*domain.Product, error) {
	const op = "cache.cart.GetProducts"

	key := getCartKey(customerId)
	values, err := c.Client.LRange(ctx, key, 0, -1).Result()
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	products := make([]*domain.Product, len(values))
	for _, val := range values {
		product := &domain.Product{}
		err = json.Unmarshal([]byte(val), product)
		if err != nil {
			return nil, formatter.FmtError(op, err)
		}

		products = append(products, product)
	}

	err = c.updateCartExpirationTime(ctx, key)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return products, nil
}

// Удалить товар из корзины покупателя
func (c *Cache) DeleteProduct(ctx context.Context, customerId, productId int) error {
	const op = "cache.cart.DeleteProduct"

	key := getCartKey(customerId)
	_, p, err := c.getProductWithIndex(ctx, key, productId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	val, err := json.Marshal(p)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = c.Client.LRem(ctx, key, 1, val).Err()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Изменить количество товара в корзине покупателя
func (c *Cache) ChangeProductCount(ctx context.Context, customerId, productId, countToAdd int) error {
	const op = "cache.cart.ChangeProductCount"

	key := getCartKey(customerId)
	i, p, err := c.getProductWithIndex(ctx, key, productId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	p.AddCount(countToAdd)
	val, err := json.Marshal(p)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = c.Client.LSet(ctx, key, int64(i), val).Err()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = c.updateCartExpirationTime(ctx, key)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Очистить корзину покупателя
func (c *Cache) Clear(ctx context.Context, customerId int) error {
	const op = "cache.cart.Clear"

	key := getCartKey(customerId)
	err := c.Client.Del(ctx, key).Err()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func getCartKey(customerId int) string {
	return fmt.Sprintf(cartKey, customerId)
}

// Получает товар и его индекс из корзины покупателя
func (c *Cache) getProductWithIndex(ctx context.Context, key string, productId int) (int, *Product, error) {
	const op = "cache.cart.getProduct"

	values, err := c.Client.LRange(ctx, key, 0, -1).Result()
	if err != nil {
		return -1, nil, formatter.FmtError(op, err)
	}

	var findedIndex int
	var findedProduct *Product
	for i, val := range values {
		product := &Product{}
		err := json.Unmarshal([]byte(val), product)
		if err != nil {
			return -1, nil, formatter.FmtError(op, err)
		}

		if product.Id == productId {
			findedIndex = i
			findedProduct = product
			break
		}
	}

	return findedIndex, findedProduct, nil
}

func (c *Cache) updateCartExpirationTime(ctx context.Context, key string) error {
	return c.Client.Expire(ctx, key, cartLiveTime).Err()
}
