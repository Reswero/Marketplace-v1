package usecase

import "context"

// Взаимодействие с оплатами заказов пользователей
type Payments interface {
	// Получить ссылку на оплату заказа
	GetLink(ctx context.Context, orderId int64, paybleAmount int) (string, error)
}
