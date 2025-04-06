package repository

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
)

type Payments interface {
	AddPendingPayment(ctx context.Context, payment *payment.Payment) error
	GetPendingPayment(ctx context.Context, paymentId string) (*payment.Payment, error)
	GetPendingPaymentByOrderId(ctx context.Context, orderId int64) (*payment.Payment, error)
	DeletePendingPayment(ctx context.Context, paymentId string) error
	CheckPendingPaymentExists(ctx context.Context, paymentId string) (bool, error)
	AddPaidPayment(ctx context.Context, payment *payment.Payment) error
}
