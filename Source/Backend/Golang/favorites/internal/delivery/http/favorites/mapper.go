package favorites

import "github.com/Reswero/Marketplace-v1/favorites/internal/usecase"

func MapFavoriteProductVmToDto(customerId int, vm *FavoriteProductVm) *usecase.FavoriteProductDto {
	return &usecase.FavoriteProductDto{
		CustomerId: customerId,
		ProductId:  vm.ProductId,
	}
}
