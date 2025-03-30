package order

// Заказ
type Order struct {
	// Идентификатор
	Id int64
	// Сумма к оплате
	PaymentAmount int
}

func New(id int64, paymentAmount int) *Order {
	return &Order{
		Id:            id,
		PaymentAmount: paymentAmount,
	}
}
