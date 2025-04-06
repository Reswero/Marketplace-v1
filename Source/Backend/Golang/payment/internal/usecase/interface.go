package usecase

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
	"github.com/Reswero/Marketplace-v1/payment/internal/domain/payment"
)

// Взаимодействие с платежами пользователей
type Payments interface {
	// Создать платёж. Возвращает идентификатор платежа
	CreatePayment(ctx context.Context, order *order.Order) (string, error)
	// Получить платёж
	GetPayment(ctx context.Context, orderId int64) (*payment.Payment, error)
	// Подтвердить платёж
	ConfirmPayment(ctx context.Context, paymentId string) error
}
