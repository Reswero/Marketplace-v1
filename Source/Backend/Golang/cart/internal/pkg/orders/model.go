package orders

// Заказ
type orderDto struct {
	// Идентификатор покупателя
	CustomerId int `json:"customerId"`
	// Товары
	Products []*productDto `json:"products"`
}

// Товар
type productDto struct {
	// Идентификатор
	Id int `json:"id"`
	// Количество
	Quantity int `json:"quantity"`
}
