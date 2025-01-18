package cart

// Товар в корзине
type Product struct {
	// Идентификатор
	Id int `json:"productId"`
	// Количество
	Count int `json:"count"`
}

func NewProduct(customerId, productId int) *Product {
	return &Product{
		Id:    productId,
		Count: 1,
	}
}

func (p *Product) AddCount(count int) {
	p.Count += count
}
