package http

import (
	"fmt"
	"log/slog"

	_ "github.com/Reswero/Marketplace-v1/cart/internal/delivery/http/docs"
	"github.com/Reswero/Marketplace-v1/cart/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/validation"
	"github.com/labstack/echo/v4"
	echoSwagger "github.com/swaggo/echo-swagger"
)

// Веб АПИ
type Delivery struct {
	logger *slog.Logger
	ucCart usecase.Cart
	router *echo.Echo
}

func New(logger *slog.Logger, env string, ucCart usecase.Cart) *Delivery {
	d := &Delivery{
		logger: logger,
		ucCart: ucCart,
	}

	d.router = echo.New()
	d.router.Validator = validation.NewValidator()

	if env == "dev" {
		d.router.GET("/swagger/*", echoSwagger.WrapHandler)
	}

	d.AddCartRoutes()

	return d
}

// Запустить http сервер
func (d *Delivery) Start(address string) error {
	return d.router.Start(address)
}

// Остановить http сервер
func (d *Delivery) Close() error {
	return d.router.Close()
}

// Залоггировать ошибку
func (d *Delivery) logError(op, message string, err error) {
	err = fmt.Errorf("%s: %w", op, err)
	d.logger.Error(message, slog.String("error", err.Error()))
}
