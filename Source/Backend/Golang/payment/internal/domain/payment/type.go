package payment

import (
	"time"

	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
)

// Платёж
type Payment struct {
	// Заказ
	Order *order.Order
	// Идентфикатор
	Id string
	// Действительно до
	ValidUntil time.Time
}

func New(order *order.Order, id string) *Payment {
	return &Payment{
		Order:      order,
		Id:         id,
		ValidUntil: time.Now().Add(time.Minute * 30),
	}
}
