package session

import (
	"context"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
)

type Manager interface {
	CreateSession(ctx context.Context, acc *account.Account) (string, error)
}
