package main

import (
	"log/slog"
	"os"

	"github.com/Reswero/Marketplace-v1/auth/internal/config"
	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http"
	sessionRedis "github.com/Reswero/Marketplace-v1/auth/internal/pkg/session/redis"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/users"
	accountRepository "github.com/Reswero/Marketplace-v1/auth/internal/repository/account"
	accountUsecase "github.com/Reswero/Marketplace-v1/auth/internal/usecase/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase/session"
	"github.com/Reswero/Marketplace-v1/pkg/outbox"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

// @title Auth Service
// @version 1.0
// @description Сервис аутентификации, авторизации и регистрации пользователей

// @securityDefinitions.apikey SessionId
// @in header
// @name Authorization

// @securityDefinitions.apikey AccountId
// @in header
// @name X-Account-Id

// @securityDefinitions.apikey AccountType
// @in header
// @name X-Account-Type

// @host localhost:8085
// @BasePath /v1
func main() {
	cfg := config.MustLoad()

	handler := slog.NewJSONHandler(os.Stdout, &slog.HandlerOptions{Level: slog.LevelDebug})
	logger := slog.New(handler)

	storage, err := postgres.New(cfg.Db.ConnString)
	if err != nil {
		logger.Error("failed to create postgres connection", slog.String("error", err.Error()))
		panic(err)
	}
	defer storage.Close()

	cache, err := redis.New(cfg.Cache.Address)
	if err != nil {
		logger.Error("failed to create redis connection", slog.String("error", err.Error()))
		panic(err)
	}
	defer cache.Close()

	accRepo := accountRepository.New(storage)
	sessManager := sessionRedis.New(cache)

	usersOutbox, err := outbox.NewDbQueue[users.OutboxDto](users.OutboxName)
	if err != nil {
		logger.Error("failed to create outbox", slog.String("error", err.Error()))
		panic(err)
	}
	defer usersOutbox.Close()

	usersService := users.New(cfg.Users.Timeout, cfg.Users.Address)

	usersOutboxDaemon := users.NewOutboxDaemon(logger, usersOutbox, accRepo)
	go usersOutboxDaemon.Start()
	defer usersOutboxDaemon.Stop()

	ucAccount := accountUsecase.New(accRepo, usersService, usersOutbox)
	ucSession := session.New(sessManager)

	d := http.New(logger, cfg.Environment, ucAccount, ucSession)
	if err := d.Start(cfg.HttpServer.Address); err != nil {
		logger.Error("failed while running http server", slog.String("error", err.Error()))
		panic(err)
	}
}
