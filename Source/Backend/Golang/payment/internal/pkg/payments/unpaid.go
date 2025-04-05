package payments

import (
	"context"
	"log/slog"
	"time"

	"github.com/Reswero/Marketplace-v1/payment/internal/repository/payments"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

type UnpaidPaymentsDaemon struct {
	exit   chan interface{}
	logger *slog.Logger
	repo   *payments.Repository
}

func NewUnpaidPaymentsDaemon(logger *slog.Logger, repo *payments.Repository) *UnpaidPaymentsDaemon {
	exit := make(chan interface{})

	return &UnpaidPaymentsDaemon{
		exit:   exit,
		logger: logger,
		repo:   repo,
	}
}

func (d *UnpaidPaymentsDaemon) Start() error {
	for {
		select {
		case <-d.exit:
			return nil
		case <-time.After(5 * time.Second):
			err := d.processUnpaidPayments()
			if err != nil {
				d.logger.Error("Error while processing unpaid payments", slog.String("error", err.Error()))
			}
		}
	}
}

func (d *UnpaidPaymentsDaemon) Stop() {
	d.exit <- struct{}{}
}

func (d *UnpaidPaymentsDaemon) processUnpaidPayments() error {
	const op = "pkg.payments.UnpaidPayments.processUnpaidPayments"

	payments, err := d.repo.GetPendingPayments(context.Background())
	if err != nil {
		return formatter.FmtError(op, err)
	}

	for _, payment := range payments {
		now := time.Now().UTC()
		dur := now.Sub(payment.ValidUntil)
		if time.Now().Sub(payment.ValidUntil) < 0 {
			continue
		}
		_ = dur

		err = d.repo.DeletePendingPayment(context.Background(), payment.Id)
		if err != nil {
			return formatter.FmtError(op, err)
		}
	}

	return nil
}
