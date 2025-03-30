package http

import (
	"log/slog"

	"github.com/Reswero/Marketplace-v1/payment/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/validation"
	"github.com/labstack/echo/v4"
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

	return d
}

func (d *Delivery) Start(address string) error {
	return d.router.Start(address)
}

func (d *Delivery) logError(op, message string, err error) {
	err = formatter.FmtError(op, err)
	d.logger.Error(message, slog.String("error", err.Error()))
}
