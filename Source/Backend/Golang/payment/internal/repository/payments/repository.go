package payments

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
)

const (
	pendingOrdersTable = "pending_payments"
	paidOrdersTable    = "paid_payments"
)

type Repository struct {
	*postgres.Storage
}

func New(s *postgres.Storage) *Repository {
	return &Repository{
		Storage: s,
	}
}

func (r *Repository) AddPendingPayment(ctx context.Context, payment payment.Payment) error {
	const op = "repository.payments.AddPendingOrder"

	sql, args, err := r.Builder.Insert(pendingOrdersTable).
		Columns("order_id", "payment_amount", "payment_id", "valid_until").
		Values(payment.Order.Id, payment.Order.PaymentAmount, payment.Id, payment.ValidUntil).
		ToSql()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
