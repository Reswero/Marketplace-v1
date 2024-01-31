package usecase

import (
	"context"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
)

type Account interface {
	CreateCustomer(ctx context.Context, acc *CustomerAccountDto) (int, error)
	CreateSeller(ctx context.Context, acc *SellerAccountDto) (int, error)
	CreateStaff(ctx context.Context, acc *StaffAccountDto) (int, error)
	CreateAdmin(ctx context.Context, acc *AdminAccountDto) (int, error)

	Get(ctx context.Context, id int) (*account.Account, error)
	Update(ctx context.Context, acc *account.Account) error
}
