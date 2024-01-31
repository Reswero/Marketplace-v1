package account

import (
	"context"
	"fmt"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/password"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase/adapters/repository"
)

type UseCase struct {
	repo repository.Account
}

func New(r repository.Account) *UseCase {
	return &UseCase{
		repo: r,
	}
}

// Создание аккаунта покупателя
func (u *UseCase) CreateCustomer(ctx context.Context, acc *usecase.CustomerAccountDto) (int, error) {
	const op = "usecase.account.CreateCustomer"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	dAcc := account.New(
		account.Customer,
		acc.PhoneNumber,
		acc.Email,
		pass,
		salt,
	)

	id, err := u.repo.CreateAccount(ctx, dAcc)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	// Call to Users Microservice
	// CustomerAccount { AccountId: id }

	return id, nil
}

// Создание аккаунта продавца
func (u *UseCase) CreateSeller(ctx context.Context, acc *usecase.CustomerAccountDto) (int, error) {
	const op = "usecase.account.CreateSeller"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	dAcc := account.New(
		account.Seller,
		acc.PhoneNumber,
		acc.Email,
		pass,
		salt,
	)

	id, err := u.repo.CreateAccount(ctx, dAcc)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	// Call to Users Microservice
	// SellerAccount { AccountId: id }

	return id, nil
}

// Создание аккаунта персонала
func (u *UseCase) CreateStaff(ctx context.Context, acc *usecase.CustomerAccountDto) (int, error) {
	const op = "usecase.account.CreateStaff"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	dAcc := account.New(
		account.Staff,
		acc.PhoneNumber,
		acc.Email,
		pass,
		salt,
	)

	id, err := u.repo.CreateAccount(ctx, dAcc)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	// Call to Users Microservice
	// StaffAccount { AccountId: id }

	return id, nil
}

// Создание аккаунта администратора
func (u *UseCase) CreateAdmin(ctx context.Context, acc *usecase.CustomerAccountDto) (int, error) {
	const op = "usecase.account.CreateAdmin"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	dAcc := account.New(
		account.Administrator,
		acc.PhoneNumber,
		acc.Email,
		pass,
		salt,
	)

	id, err := u.repo.CreateAccount(ctx, dAcc)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	// Call to Users Microservice
	// AdministratorAccount { AccountId: id }

	return id, nil
}

// Получение аккаунта по идентификатору
func (u *UseCase) Get(ctx context.Context, id int) (*account.Account, error) {
	const op = "usecase.account.Get"

	acc, err := u.repo.GetAccount(ctx, id)
	if err != nil {
		return nil, fmt.Errorf("%s: %w", op, err)
	}

	return acc, nil
}

// Обновление аккаунта
func (u *UseCase) Update(ctx context.Context, acc *account.Account) error {
	const op = "usecase.account.Update"

	err := u.repo.UpdateAccount(ctx, acc)
	if err != nil {
		return fmt.Errorf("%s: %w", op, err)
	}

	return nil
}
