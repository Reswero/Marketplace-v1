package administrator

import "github.com/Reswero/Marketplace-v1/auth/internal/domain/person"

// Администратор
type Administrator struct {
	// Идентификатор аккаунта
	AccountId int
	// Личная информация
	person.Person
}
