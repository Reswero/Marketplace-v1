package account

import (
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/contact"
)

// Тип аккаунта
type AccountType string

const (
	// Покупатель
	Customer AccountType = "customer"
	// Продавец
	Seller AccountType = "seller"
	// Персонал
	Staff AccountType = "staff"
	// Администратор
	Administrator AccountType = "admin"
)

// Аккаунт
type Account struct {
	// Идентификатор
	Id int
	// Тип
	Type AccountType
	// Контактная информация
	contact.ContactInfo
	// Дата создания
	CreatedAt time.Time
	// Хэшированный пароль
	Password string
	// Соль пароля
	Salt string
}
