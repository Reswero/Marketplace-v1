package repository

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
)

type Payments interface {
	AddPendingPayment(ctx context.Context, payment *payment.Payment) error
	GetPendingPayment(ctx context.Context, orderId int64) (*payment.Payment, error)
	DeletePendingPayment(ctx context.Context, orderId int64) error
	CheckPaymentIdExists(ctx context.Context, paymentId string) (bool, error)
	AddPaidPayment(ctx context.Context, payment *payment.Payment) error
}
