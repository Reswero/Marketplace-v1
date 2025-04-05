package usecase

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
)

// Взаимодействие с платежами пользователей
type Payments interface {
	// Создать платёж. Возвращает идентификатор платежа
	CreatePayment(ctx context.Context, order *order.Order) (string, error)
	// Подтвердить платёж
	ConfirmPayment(ctx context.Context, paymentId string) error
}
