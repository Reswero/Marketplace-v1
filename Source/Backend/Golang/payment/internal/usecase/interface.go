package usecase

import (
	"context"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
)

// Взаимодействие с платежами пользователей
type Payments interface {
	// Получить ссылку на платёж
	GetLink(ctx context.Context, order *order.Order) (string, error)
}
