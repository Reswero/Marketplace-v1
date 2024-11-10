package http

import (
	"fmt"
	"log/slog"

	"github.com/Reswero/Marketplace-v1/favorites/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/validation"
	"github.com/labstack/echo/v4"
	echoSwagger "github.com/swaggo/echo-swagger"
)

// АПИ
type Delivery struct {
	router      *echo.Echo
	logger      *slog.Logger
	ucFavorites usecase.Favorites
}

func New(logger *slog.Logger, env string, ucFavorites usecase.Favorites) *Delivery {
	d := &Delivery{
		logger:      logger,
		ucFavorites: ucFavorites,
	}

	d.router = echo.New()
	d.router.Validator = validation.NewValidator()

	if env == "dev" {
		d.router.GET("/swagger/*", echoSwagger.WrapHandler)
	}

	d.AddFavoritesRoutes()

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
