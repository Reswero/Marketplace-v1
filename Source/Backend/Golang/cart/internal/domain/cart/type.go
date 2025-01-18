package cart

// Товар в корзине
type Product struct {
	// Идентификатор покупателя
	CustomerId int
	// Идентификатор товара
	ProductId int
	// Количество
	Count int
}
