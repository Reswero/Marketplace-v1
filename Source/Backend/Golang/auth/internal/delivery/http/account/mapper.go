package account

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
)

func MapToCustomerAccountDto(vm *CustomerAccountCreateVm) *usecase.CustomerAccountDto {
	return &usecase.CustomerAccountDto{
		AccountDto: usecase.AccountDto{
			PhoneNumber: vm.PhoneNumber,
			Email:       vm.Email,
			Password:    vm.Password,
		},
		PersonDto: usecase.PersonDto{
			FirstName: vm.FirstName,
			LastName:  vm.LastName,
		},
	}
}

func MapToSellerAccountDto(vm *SellerAccountCreateVm) *usecase.SellerAccountDto {
	return &usecase.SellerAccountDto{
		AccountDto: usecase.AccountDto{
			PhoneNumber: vm.PhoneNumber,
			Email:       vm.Email,
			Password:    vm.Password,
		},
		PersonDto: usecase.PersonDto{
			FirstName: vm.FirstName,
			LastName:  vm.LastName,
		},
		CompanyName: vm.CompanyName,
	}
}

func MapToStaffAccountDto(vm *StaffAccountCreateVm) *usecase.StaffAccountDto {
	return &usecase.StaffAccountDto{
		AccountDto: usecase.AccountDto{
			PhoneNumber: vm.PhoneNumber,
			Email:       vm.Email,
			Password:    vm.Password,
		},
		PersonDto: usecase.PersonDto{
			FirstName: vm.FirstName,
			LastName:  vm.LastName,
		},
	}
}

func MapToAdminAccountDto(vm *AdminAccountCreateVm) *usecase.AdminAccountDto {
	return &usecase.AdminAccountDto{
		AccountDto: usecase.AccountDto{
			PhoneNumber: vm.PhoneNumber,
			Email:       vm.Email,
			Password:    vm.Password,
		},
		PersonDto: usecase.PersonDto{
			FirstName: vm.FirstName,
			LastName:  vm.LastName,
		},
	}
}

func MapToAccountVm(acc *account.Account) *AccountVm {
	return &AccountVm{
		Id:          acc.Id,
		PhoneNumber: acc.PhoneNumber,
		Email:       acc.Email,
	}
}
