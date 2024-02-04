package usecase

import (
	"context"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/session"
)

type Account interface {
	CreateCustomer(ctx context.Context, acc *CustomerAccountDto) (int, error)
	CreateSeller(ctx context.Context, acc *SellerAccountDto) (int, error)
	CreateStaff(ctx context.Context, acc *StaffAccountDto) (int, error)
	CreateAdmin(ctx context.Context, acc *AdminAccountDto) (int, error)

	Get(ctx context.Context, id int) (*account.Account, error)
	Update(ctx context.Context, acc *account.Account) error

	ValidateCredentials(ctx context.Context, dto *CredentialsDto) (*account.Account, bool, error)

	ChangePassword(ctx context.Context, dto *ChangePasswordDto) (bool, error)
	ChangeEmail(ctx context.Context, dto *ChangeEmailDto) (bool, error)
}

type Session interface {
	Create(ctx context.Context, acc *account.Account) (string, error)
	Get(ctx context.Context, id string) (*session.Session, error)
	Delete(ctx context.Context, id string) error
}
