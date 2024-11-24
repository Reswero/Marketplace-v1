package usecase

// Модель избранного товара
type FavoriteProductDto struct {
	// Идентификатор покупателя
	CustomerId int
	// Идентификатор товара
	ProductId int
}

// Модель пагинации
type PaginationDto struct {
	// Смещение
	Offset int
	// Ограничение
	Limit int
}
