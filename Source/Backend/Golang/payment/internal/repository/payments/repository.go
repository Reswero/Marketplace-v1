package payments

import (
	"context"

	"github.com/Masterminds/squirrel"
	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
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

// Получить ожидающий платёж
func (r *Repository) GetPendingPayment(ctx context.Context, orderId int64) (*payment.Payment, error) {
	const op = "repository.payments.GetPendingPayment"

	sql, args, err := r.Builder.Select("order_id, payment_amount, payment_id, valid_until").
		From(pendingPaymentsTable).
		Where(squirrel.Eq{"order_id": orderId}).
		ToSql()
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	row := r.Pool.QueryRow(ctx, sql, args...)

	payment := &payment.Payment{
		Order: &order.Order{},
	}
	err = row.Scan(&payment.Order.Id, &payment.Order.PaymentAmount,
		&payment.Id, &payment.ValidUntil)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return payment, nil
}

func (r *Repository) DeletePendingPayment(ctx context.Context, orderId int64) error {
	const op = "repository.payments.DeletePendingPayment"

	sql, args, err := r.Builder.Delete(pendingPaymentsTable).
		Where(squirrel.Eq{"order_id": orderId}).
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

func (r *Repository) CheckPendingPaymentExists(ctx context.Context, paymentId string) (bool, error) {
	const op = "repository.payments.CheckPendingPaymentExists"

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

func (r *Repository) AddPaidPayment(ctx context.Context, payment *payment.Payment) error {
	const op = "repository.payments.AddPaidPayment"

	sql, args, err := r.Builder.Insert(paidPaymentsTable).
		Columns("order_id, payment_amount, paid_at").
		Values(payment.Order.Id, payment.Order.PaymentAmount, payment.PaidAt).
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
