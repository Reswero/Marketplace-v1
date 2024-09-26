package users

import (
	"context"
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/repository/account"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/outbox"
)

const OutboxName = "users.db"

type OutboxDaemon struct {
	exit   chan interface{}
	outbox *outbox.DbQueue[OutboxDto]
	repo   *account.Repository
}

func NewOutboxDaemon(outbox *outbox.DbQueue[OutboxDto], repo *account.Repository) *OutboxDaemon {
	exit := make(chan interface{})

	return &OutboxDaemon{
		outbox: outbox,
		repo:   repo,
		exit:   exit,
	}
}

func (o *OutboxDaemon) Start() error {
	select {
	case <-o.exit:
		return nil
	case <-time.After(5 * time.Second):
		o.processOutbox()
	}

	return nil
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
	value, _ := o.outbox.Peek()

	switch value.Action {
	case DeleteAction:
		o.repo.DeleteAccount(context.Background(), value.AccountId)
	}
}
