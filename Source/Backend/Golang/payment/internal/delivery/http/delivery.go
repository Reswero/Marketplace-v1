package http

import (
	"log/slog"

	_ "github.com/Reswero/Marketplace-v1/payment/internal/delivery/http/docs"
	"github.com/Reswero/Marketplace-v1/payment/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/validation"
	"github.com/labstack/echo/v4"
	echoSwagger "github.com/swaggo/echo-swagger"
)

type Delivery struct {
	logger     *slog.Logger
	router     *echo.Echo
	ucPayments usecase.Payments
}

func New(logger *slog.Logger, env string, ucPayments usecase.Payments) *Delivery {
	d := &Delivery{
		logger:     logger,
		router:     echo.New(),
		ucPayments: ucPayments,
	}

	d.router.Validator = validation.NewValidator()
	d.AddPaymentRoutes()

	if env == "dev" {
		d.router.GET("/swagger/*", echoSwagger.WrapHandler)
	}

	return d
}

func (d *Delivery) Start(address string) error {
	return d.router.Start(address)
}

func (d *Delivery) logError(op, message string, err error) {
	err = formatter.FmtError(op, err)
	d.logger.Error(message, slog.String("error", err.Error()))
}
