package main

import (
	"log/slog"
	"os"

	cartCache "github.com/Reswero/Marketplace-v1/cart/internal/cache/cart"
	"github.com/Reswero/Marketplace-v1/cart/internal/config"
	"github.com/Reswero/Marketplace-v1/cart/internal/delivery/http"
	cartUsecase "github.com/Reswero/Marketplace-v1/cart/internal/usecase/cart"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

// @title Cart Service
// @version 1.0
// @description Сервис корзины

// @host localhost:8089
// @basePath /v1

// @securityDefinitions.apiKey AccountId
// @in header
// @name X-Account-Id

// @securityDefinitions.apiKey AccountType
// @in header
// @name X-Account-Type
func main() {
	cfg := config.MustLoad()

	handler := slog.NewJSONHandler(os.Stdout, &slog.HandlerOptions{Level: slog.LevelDebug})
	logger := slog.New(handler)

	cache, err := redis.New(cfg.Cache.Address)
	if err != nil {
		logger.Error("failed to create redis connection", slog.String("error", err.Error()))
		panic(err)
	}

	cartCache := cartCache.New(cache)
	ucCart := cartUsecase.New(cartCache)

	d := http.New(logger, cfg.Environment, ucCart)
	if err = d.Start(cfg.HttpServer.Address); err != nil {
		logger.Error("failed to start http server", slog.String("error", err.Error()))
		panic(err)
	}
}
