package main

import (
	"log/slog"
	"os"

	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http"
	sessionRedis "github.com/Reswero/Marketplace-v1/auth/internal/pkg/session/redis"
	accountRepository "github.com/Reswero/Marketplace-v1/auth/internal/repository/account"
	accountUsecase "github.com/Reswero/Marketplace-v1/auth/internal/usecase/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase/session"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

func main() {
	handler := slog.NewJSONHandler(os.Stdout, &slog.HandlerOptions{Level: slog.LevelDebug})
	logger := slog.New(handler)

	storage, err := postgres.New("postgres://postgres:root@localhost:5432/go-auth")
	if err != nil {
		logger.Error("failed to create postgres connection", slog.String("error", err.Error()))
		panic(err)
	}
	defer storage.Close()

	cache, err := redis.New("localhost:6379")
	if err != nil {
		logger.Error("failed to create redis connection", slog.String("error", err.Error()))
		panic(err)
	}
	defer cache.Close()

	accRepo := accountRepository.New(storage)
	sessManager := sessionRedis.New(cache)

	ucAccount := accountUsecase.New(accRepo)
	ucSession := session.New(sessManager)

	d := http.New(logger, ucAccount, ucSession)
	if err := d.Start(":8085"); err != nil {
		logger.Error("failed while running http server", slog.String("error", err.Error()))
		panic(err)
	}
}
