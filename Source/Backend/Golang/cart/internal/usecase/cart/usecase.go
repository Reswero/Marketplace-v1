package cart

import (
	"context"

	domain "github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
	"github.com/Reswero/Marketplace-v1/cart/internal/pkg/orders"
	"github.com/Reswero/Marketplace-v1/cart/internal/usecase"
	"github.com/Reswero/Marketplace-v1/cart/internal/usecase/adapters/cache"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/products"
)

type Usecase struct {
	cache    cache.Cart
	products *products.Service
	orders   *orders.Client
}

func New(cache cache.Cart, products *products.Service, orders *orders.Client) *Usecase {
	return &Usecase{
		cache:    cache,
		products: products,
		orders:   orders,
	}
}

func (u *Usecase) AddProduct(ctx context.Context, customerId, productId int) error {
	const op = "usecase.cart.AddProduct"

	existingIds, err := u.products.CheckProductsExistence(ctx, []int{productId})
	if err != nil {
		return formatter.FmtError(op, err)
	}

	if len(existingIds) < 1 {
		return usecase.ErrProductNotExists
	}

	err = u.cache.AddProduct(ctx, customerId, productId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (u *Usecase) GetProducts(ctx context.Context, customerId int) ([]*domain.Product, error) {
	const op = "usecase.cart.GetProducts"

	products, err := u.cache.GetProducts(ctx, customerId)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return products, nil
}

func (u *Usecase) DeleteProduct(ctx context.Context, customerId, productId int) error {
	const op = "usecase.cart.DeleteProduct"

	err := u.cache.DeleteProduct(ctx, customerId, productId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (u *Usecase) ChangeProductCount(ctx context.Context, customerId, productId, countToAdd int) error {
	const op = "usecase.cart.ChangeProductCount"

	err := u.cache.ChangeProductCount(ctx, customerId, productId, countToAdd)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (u *Usecase) Clear(ctx context.Context, customerId int) error {
	const op = "usecase.cart.Clear"

	err := u.cache.Clear(ctx, customerId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (u *Usecase) Checkout(ctx context.Context, customerId int) error {
	const op = "usecase.cart.Checkout"

	products, err := u.cache.GetProducts(ctx, customerId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.orders.CreateOrder(products)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.cache.Clear(ctx, customerId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
