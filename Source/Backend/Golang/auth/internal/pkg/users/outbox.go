package users

import (
	"context"
	"database/sql"
	"errors"
	"log/slog"
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/repository/account"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/outbox"
)

const OutboxName = "users.db"

type OutboxDaemon struct {
	exit   chan interface{}
	logger *slog.Logger
	outbox *outbox.DbQueue[OutboxDto]
	repo   *account.Repository
}

func NewOutboxDaemon(logger *slog.Logger, outbox *outbox.DbQueue[OutboxDto], repo *account.Repository) *OutboxDaemon {
	exit := make(chan interface{})

	return &OutboxDaemon{
		exit:   exit,
		logger: logger,
		outbox: outbox,
		repo:   repo,
	}
}

func (o *OutboxDaemon) Start() error {
	for {
		select {
		case <-o.exit:
			return nil
		case <-time.After(5 * time.Second):
			o.processOutbox()
		}
	}
}

func (o *OutboxDaemon) Stop() error {
	const op = "users.OutboxDaemon.Stop"

	o.exit <- struct{}{}

	err := o.outbox.Close()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (o *OutboxDaemon) processOutbox() {
	const op = "users.OutboxDaemon.processOutbox"

	value, err := o.outbox.Peek()
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			return
		}
		o.logger.Error(op, slog.String("error", err.Error()))
	}

	switch value.Action {
	case DeleteAction:
		err = o.repo.DeleteAccount(context.Background(), value.AccountId)
	}

	if err != nil {
		o.logger.Error(op, slog.String("error", err.Error()))
	}

	_, err = o.outbox.Pop()
	if err != nil {
		o.logger.Error(op, slog.String("error", err.Error()))
	}
}
