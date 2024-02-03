package seller

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/person"
)

// Продавец
type Seller struct {
	// Идентификатор аккаунта
	AccountId int
	// Личная информация
	person.Person
	// Название компании
	CompanyName string
}
