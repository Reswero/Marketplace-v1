package order

import "time"

// Заказ
type Order struct {
	// Идентификатор
	Id int64
	// Сумма к оплате
	PaymentAmount int
	// Дата оплаты
	PaidAt time.Time
}

func New(id int64, paymentAmount int) *Order {
	return &Order{
		Id:            id,
		PaymentAmount: paymentAmount,
		PaidAt:        time.Time{},
	}
}

func NewWithPaidTime(id int64, paymentAmount int, paidAt time.Time) *Order {
	return &Order{
		Id:            id,
		PaymentAmount: paymentAmount,
		PaidAt:        paidAt,
	}
}
