package session

import (
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
)

// Пользовательская сессия
type Session struct {
	// Идентификатор аккаунта
	AccountId int `json:"accountId"`
	// Тип аккаунта
	AccountType account.AccountType `json:"accountType"`
	// Дата создания сессии
	CreateAt time.Time
}

func New(acc *account.Account) *Session {
	return &Session{
		AccountId:   acc.Id,
		AccountType: acc.Type,
		CreateAt:    time.Now(),
	}
}
