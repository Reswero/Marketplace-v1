package authorization

import (
	"net/http"
	"strconv"

	"github.com/labstack/echo/v4"
)

const (
	AuthorizationHeader = "Authorization"
	AccountIdHeader     = "X-Account-Id"
	AccountTypeHeader   = "X-Account-Type"
)

// Авторизационные данные пользователя
type Claims struct {
	// Идентификатор аккаунта
	AccountId int
	// Тип аккаунта
	AccountType string
}

// Получает авторизационные данные из заголовков
func Middleware(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		idHeader, ok := c.Request().Header[AccountIdHeader]
		if !ok {
			return c.NoContent(http.StatusUnauthorized)
		}

		accTypeHeader, ok := c.Request().Header[AccountTypeHeader]
		if !ok {
			return c.NoContent(http.StatusUnauthorized)
		}

		id, err := strconv.Atoi(idHeader[0])
		if err != nil {
			return c.NoContent(http.StatusUnauthorized)
		}

		c.Set("AccountId", id)
		c.Set("AccountType", accTypeHeader[0])

		return next(c)
	}
}

// Получает авторизационные данные из контекста
func GetClaims(c echo.Context) *Claims {
	id := c.Get("AccountId").(int)
	accType := c.Get("AccountType").(string)

	return &Claims{
		AccountId:   id,
		AccountType: accType,
	}
}
