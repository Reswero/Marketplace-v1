package cart

import (
	"context"

	domain "github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
	"github.com/Reswero/Marketplace-v1/cart/internal/usecase/adapters/cache"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

type Usecase struct {
	cache cache.Cart
}

func New(cache cache.Cart) *Usecase {
	return &Usecase{
		cache: cache,
	}
}

func (u *Usecase) AddProduct(ctx context.Context, customerId, productId int) error {
	const op = "usecase.cart.AddProduct"

	err := u.cache.AddProduct(ctx, customerId, productId)
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

	product, err := u.cache.GetProducts(ctx, customerId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_ = product
	// TODO: Call to Orders service

	err = u.cache.Clear(ctx, customerId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
