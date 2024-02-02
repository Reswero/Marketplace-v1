package session

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
)

func MapToCredentialsDto(vm *CredentialsVm) *usecase.CredentialsDto {
	return &usecase.CredentialsDto{
		PhoneNumber: vm.PhoneNumber,
		AccountType: account.AccountType(vm.AccountType),
		Password:    vm.Password,
	}
}
