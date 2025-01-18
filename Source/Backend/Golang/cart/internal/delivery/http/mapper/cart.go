package mapper

import (
	"github.com/Reswero/Marketplace-v1/cart/internal/delivery/http/viewmodel"
	domain "github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
)

func MapToProductVm(product *domain.Product) *viewmodel.Product {
	return &viewmodel.Product{
		Id:    product.ProductId,
		Count: product.Count,
	}
}
