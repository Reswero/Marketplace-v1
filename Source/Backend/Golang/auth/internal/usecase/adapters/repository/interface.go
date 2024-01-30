package repository

import (
	"context"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
)

type Account interface {
	CreateAccount(ctx context.Context, acc *account.Account) (int, error)
	GetAccount(ctx context.Context, id int) error
	UpdateAccount(ctx context.Context, acc *account.Account) error
	DeleteAccount(ctx context.Context, id int) error
}
