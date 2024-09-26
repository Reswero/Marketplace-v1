package account

import (
	"context"
	"database/sql"
	"errors"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/password"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/users"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase/adapters/repository"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/outbox"
)

type UseCase struct {
	repo         repository.Account
	usersService *users.Users
	usersOutbox  *outbox.DbQueue[users.OutboxDto]
}

func New(r repository.Account, u *users.Users, o *outbox.DbQueue[users.OutboxDto]) *UseCase {
	return &UseCase{
		repo:         r,
		usersService: u,
		usersOutbox:  o,
	}
}

// Создание аккаунта покупателя
func (u *UseCase) CreateCustomer(ctx context.Context, acc *usecase.CustomerAccountDto) (int, error) {
	const op = "usecase.account.CreateCustomer"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, formatter.FmtError(op, err)
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
		return 0, formatter.FmtError(op, err)
	}

	cAcc := &users.CreateCustomerDto{
		AccountId: id,
		FirstName: acc.FirstName,
		LastName:  acc.LastName,
	}

	err = u.usersService.CreateCustomer(ctx, cAcc)
	if err != nil {
		u.usersOutbox.Push(&users.OutboxDto{
			Action:    users.DeleteAction,
			AccountId: id,
		})
		return 0, formatter.FmtError(op, err)
	}

	return id, nil
}

// Создание аккаунта продавца
func (u *UseCase) CreateSeller(ctx context.Context, acc *usecase.SellerAccountDto) (int, error) {
	const op = "usecase.account.CreateSeller"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, formatter.FmtError(op, err)
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
		return 0, formatter.FmtError(op, err)
	}

	cAcc := &users.CreateSellerDto{
		AccountId:   id,
		FirstName:   acc.FirstName,
		LastName:    acc.LastName,
		CompanyName: acc.CompanyName,
	}

	err = u.usersService.CreateSeller(ctx, cAcc)
	if err != nil {
		u.usersOutbox.Push(&users.OutboxDto{
			Action:    users.DeleteAction,
			AccountId: id,
		})
		return 0, formatter.FmtError(op, err)
	}

	return id, nil
}

// Создание аккаунта персонала
func (u *UseCase) CreateStaff(ctx context.Context, acc *usecase.StaffAccountDto) (int, error) {
	const op = "usecase.account.CreateStaff"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, formatter.FmtError(op, err)
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
		return 0, formatter.FmtError(op, err)
	}

	cAcc := &users.CreateStaffDto{
		AccountId: id,
		FirstName: acc.FirstName,
		LastName:  acc.LastName,
	}

	err = u.usersService.CreateStaff(ctx, cAcc)
	if err != nil {
		u.usersOutbox.Push(&users.OutboxDto{
			Action:    users.DeleteAction,
			AccountId: id,
		})
		return 0, formatter.FmtError(op, err)
	}

	return id, nil
}

// Создание аккаунта администратора
func (u *UseCase) CreateAdmin(ctx context.Context, acc *usecase.AdminAccountDto) (int, error) {
	const op = "usecase.account.CreateAdmin"

	pass, salt, err := password.Hash(acc.Password)
	if err != nil {
		return 0, formatter.FmtError(op, err)
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
		return 0, formatter.FmtError(op, err)
	}

	cAcc := &users.CreateAdministratorDto{
		AccountId: id,
		FirstName: acc.FirstName,
		LastName:  acc.LastName,
	}

	err = u.usersService.CreateAdministrator(ctx, cAcc)
	if err != nil {
		u.usersOutbox.Push(&users.OutboxDto{
			Action:    users.DeleteAction,
			AccountId: id,
		})
		return 0, formatter.FmtError(op, err)
	}

	return id, nil
}

// Получение аккаунта по идентификатору
func (u *UseCase) Get(ctx context.Context, id int) (*account.Account, error) {
	const op = "usecase.account.Get"

	acc, err := u.repo.GetAccount(ctx, id)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return nil, usecase.ErrAccountNotFound
		}

		return nil, formatter.FmtError(op, err)
	}

	return acc, nil
}

// Обновление аккаунта
func (u *UseCase) Update(ctx context.Context, acc *account.Account) error {
	const op = "usecase.account.Update"

	err := u.repo.UpdateAccount(ctx, acc)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Проверяет верность учетных данных.
// В случае, если данные верные, возвращает аккаунт, иначе nil
func (u *UseCase) ValidateCredentials(ctx context.Context, dto *usecase.CredentialsDto) (*account.Account, bool, error) {
	const op = "usecase.account.ValidatePassword"

	acc, err := u.repo.GetAccountByPhoneNumber(ctx, dto.PhoneNumber, dto.AccountType)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return nil, false, usecase.ErrAccountNotFound
		}

		return nil, false, formatter.FmtError(op, err)
	}

	valid := password.Validate(acc.Password, dto.Password, acc.Salt)
	if !valid {
		return nil, false, nil
	}

	return acc, true, nil
}

// Изменяет пароль аккаунта на новый
func (u *UseCase) ChangePassword(ctx context.Context, dto *usecase.ChangePasswordDto) (bool, error) {
	const op = "usecase.account.ChangePassword"

	acc, err := u.repo.GetAccount(ctx, dto.AccountId)
	if err != nil {
		return false, formatter.FmtError(op, err)
	}

	valid := password.Validate(acc.Password, dto.OldPassword, acc.Salt)
	if !valid {
		return false, nil
	}

	pass, salt, err := password.Hash(dto.NewPassword)
	if err != nil {
		return false, formatter.FmtError(op, err)
	}
	acc.ChangePassword(pass, salt)

	err = u.repo.UpdateAccount(ctx, acc)
	if err != nil {
		return false, formatter.FmtError(op, err)
	}

	return true, nil
}

// Изменяет почту аккаунта на новую
func (u *UseCase) ChangeEmail(ctx context.Context, dto *usecase.ChangeEmailDto) (bool, error) {
	const op = "usecase.account.ChangeEmail"

	acc, err := u.repo.GetAccount(ctx, dto.AccountId)
	if err != nil {
		return false, formatter.FmtError(op, err)
	}

	valid := password.Validate(acc.Password, dto.Password, acc.Salt)
	if !valid {
		return false, nil
	}

	acc.ChangeEmail(dto.NewEmail)

	err = u.repo.UpdateAccount(ctx, acc)
	if err != nil {
		return false, formatter.FmtError(op, err)
	}

	return true, nil
}
