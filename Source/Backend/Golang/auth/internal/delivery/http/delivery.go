package http

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/labstack/echo/v4"
)

type Delivery struct {
	ucAccount usecase.Account
	router    *echo.Echo
}

func New(ucAccount usecase.Account) *Delivery {
	d := &Delivery{
		ucAccount: ucAccount,
	}

	d.router = echo.New()

	return d
}

func (d *Delivery) Start(address string) error {
	return d.router.Start(address)
}
