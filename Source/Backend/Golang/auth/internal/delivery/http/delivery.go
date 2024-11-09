package http

import (
	"fmt"
	"log/slog"

	_ "github.com/Reswero/Marketplace-v1/auth/internal/delivery/http/docs"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/validation"
	"github.com/labstack/echo/v4"
	echoSwagger "github.com/swaggo/echo-swagger"
)

type Delivery struct {
	logger    *slog.Logger
	ucAccount usecase.Account
	ucSession usecase.Session
	router    *echo.Echo
}

func New(logger *slog.Logger, env string,
	ucAccount usecase.Account, ucSession usecase.Session) *Delivery {
	d := &Delivery{
		logger:    logger,
		ucAccount: ucAccount,
		ucSession: ucSession,
	}

	d.router = echo.New()
	d.router.Validator = validation.NewValidator()

	if env == "dev" {
		d.router.GET("/swagger/*", echoSwagger.WrapHandler)
	}

	d.AddAccountRoutes()
	d.AddSessionRoutes()

	return d
}

func (d *Delivery) Start(address string) error {
	return d.router.Start(address)
}

func (d *Delivery) logError(op, message string, err error) {
	err = fmt.Errorf("%s: %w", op, err)
	d.logger.Error(message, slog.String("error", err.Error()))
}
