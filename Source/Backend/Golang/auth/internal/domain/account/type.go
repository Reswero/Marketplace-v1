package account

import (
	"time"
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
	// Номер телефона
	PhoneNumber string
	// Электронная почта
	Email string
	// Дата создания
	CreatedAt time.Time
	// Хэшированный пароль
	Password string
	// Соль пароля
	Salt string
}