package payments

import (
	"context"

	"github.com/Masterminds/squirrel"
	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
)

const (
	pendingPaymentsTable = "pending_payments"
	paidPaymentsTable    = "paid_payments"
)

type Repository struct {
	*postgres.Storage
}

func New(s *postgres.Storage) *Repository {
	return &Repository{
		Storage: s,
	}
}

// Добавить платёж в ожидание
func (r *Repository) AddPendingPayment(ctx context.Context, payment *payment.Payment) error {
	const op = "repository.payments.AddPendingOrder"

	sql, args, err := r.Builder.Insert(pendingPaymentsTable).
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

func (r *Repository) CheckPaymentIdExists(ctx context.Context, paymentId string) (bool, error) {
	const op = "repository.payments.CheckPaymentIdExists"

	sql, args, err := r.Builder.Select("payment_id").
		From(pendingPaymentsTable).
		Where(squirrel.Eq{"payment_id": paymentId}).
		ToSql()
	if err != nil {
		return false, formatter.FmtError(op, err)
	}

	rows, err := r.Pool.Query(ctx, sql, args...)
	if err != nil {
		return false, formatter.FmtError(op, err)
	}
	defer rows.Close()

	return rows.Next(), nil
}
