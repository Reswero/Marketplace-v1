package staff

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/person"
)

// Персонал
type Staff struct {
	// Идентификатор аккаунта
	AccountId int
	// Личная информация
	person.Person
}
