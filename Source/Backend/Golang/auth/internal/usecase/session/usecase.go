package session

import (
	"context"
	"errors"

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

func (u *UseCase) Get(ctx context.Context, id string) (*session.Session, error) {
	const op = "usecase.session.Get"

	sess, err := u.manager.GetSession(ctx, id)
	if err != nil {
		if errors.Is(err, session.ErrSessionNotFound) {
			return nil, err
		}

		return nil, formatter.FmtError(op, err)
	}

	return sess, nil
}

func (u *UseCase) Delete(ctx context.Context, id string) error {
	const op = "usecase.session.Delete"

	err := u.manager.DeleteSession(ctx, id)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
