package payments

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
	"github.com/Reswero/Marketplace-v1/payment/internal/usecase/adapters/repository"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/generator"
)

const (
	paymentIdLength = 20
)

type UseCase struct {
	repo repository.Payments
}

func New(repo repository.Payments) *UseCase {
	return &UseCase{
		repo: repo,
	}
}

func (u *UseCase) CreatePayment(ctx context.Context, order *order.Order) (string, error) {
	const op = "usecase.payments.CreatePayment"

	id := ""
	for id == "" {
		id, err := generator.GetRandomAsciiString(paymentIdLength)
		if err != nil {
			return "", formatter.FmtError(op, err)
		}

		exists, err := u.repo.CheckPendingPaymentExists(ctx, id)
		if err != nil {
			return op, formatter.FmtError(op, err)
		}

		if exists {
			continue
		}
	}

	payment := payment.New(order, id)
	err := u.repo.AddPendingPayment(ctx, payment)
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	return payment.Id, nil
}

func (u *UseCase) ConfirmPayment(ctx context.Context, paymentId string) error {
	const op = "usecase.payments.ConfirmPayment"

	payment, err := u.repo.GetPendingPayment(ctx, paymentId)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	payment.Confirm()

	err = u.repo.DeletePendingPayment(ctx, payment.Id)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.repo.AddPaidPayment(ctx, payment)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
