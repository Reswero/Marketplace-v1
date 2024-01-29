package customer

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/contact"
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/person"
)

// Покупатель
type Customer struct {
	// Идентификатор аккаунта
	AccountId int
	// Личная информация
	person.Person
	// Контактная информация
	contact.ContactInfo
}
