package payments

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
	"github.com/Reswero/Marketplace-v1/payment/internal/pkg/orders"
	"github.com/Reswero/Marketplace-v1/payment/internal/usecase/adapters/repository"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/generator"
)

const (
	paymentIdLength = 20
)

type UseCase struct {
	repo         repository.Payments
	ordersClient *orders.Client
}

func New(repo repository.Payments, o *orders.Client) *UseCase {
	return &UseCase{
		repo:         repo,
		ordersClient: o,
	}
}

func (u *UseCase) CreatePayment(ctx context.Context, order *order.Order) (id string, err error) {
	const op = "usecase.payments.CreatePayment"

	id = ""
	for id == "" {
		id, err = generator.GetRandomAsciiString(paymentIdLength)
		if err != nil {
			return "", formatter.FmtError(op, err)
		}

		exists, err := u.repo.CheckPendingPaymentExists(ctx, id)
		if err != nil {
			return op, formatter.FmtError(op, err)
		}

		if exists {
			id = ""
		}
	}

	payment := payment.New(order, id)
	err = u.repo.AddPendingPayment(ctx, payment)
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	return payment.Id, nil
}

func (u *UseCase) GetPayment(ctx context.Context, orderId int64) (*payment.Payment, error) {
	const op = "usecase.payments.GetPayment"

	payment, err := u.repo.GetPendingPaymentByOrderId(ctx, orderId)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return payment, nil
}

func (u *UseCase) ConfirmPayment(ctx context.Context, paymentId string) error {
	const op = "usecase.payments.ConfirmPayment"

	payment, err := u.repo.GetPendingPayment(ctx, paymentId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	payment.Confirm()

	// TODO: add transaction

	err = u.repo.DeletePendingPayment(ctx, payment.Id)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.repo.AddPaidPayment(ctx, payment)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.ordersClient.ConfirmPayment(ctx, payment.Order.Id)
	if err != nil {
		// TODO: Add to outbox
		return formatter.FmtError(op, err)
	}

	return nil
}
