package repository

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
)

type Payments interface {
	AddPendingPayment(ctx context.Context, payment payment.Payment) error
	CheckPaymentIdExists(ctx context.Context, paymentId string) (bool, error)
}
