package session

import (
	"context"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/session"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

type UseCase struct {
	manager session.Manager
}

func New(manager session.Manager) *UseCase {
	return &UseCase{
		manager: manager,
	}
}

func (u *UseCase) Create(ctx context.Context, acc *account.Account) (string, error) {
	const op = "usecase.session.Create"

	id, err := u.manager.CreateSession(ctx, acc)
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	return id, nil
}
