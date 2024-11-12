package main

import (
	"embed"
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
	"github.com/jackc/pgx/v5/stdlib"
	"github.com/pressly/goose/v3"
)

//go:embed migrations
var embedMigrations embed.FS

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

	storageConfig := &postgres.Config{
		IpAddress: cfg.Db.IpAddress,
		Port:      cfg.Db.Port,
		User:      cfg.Db.User,
		Password:  cfg.Db.Password,
		DbName:    cfg.Db.DbName,
	}
	storage, err := postgres.New(storageConfig)
	if err != nil {
		logger.Error("failed to create postgres connection", slog.String("error", err.Error()))
		panic(err)
	}
	defer storage.Close()

	err = migrateDb(storage)
	if err != nil {
		logger.Error("failed to migrate db", slog.String("error", err.Error()))
		panic(err)
	}

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

func migrateDb(storage *postgres.Storage) error {
	goose.SetBaseFS(embedMigrations)

	err := goose.SetDialect("postgres")
	if err != nil {
		return err
	}

	db := stdlib.OpenDBFromPool(storage.Pool)
	defer db.Close()

	return goose.Up(db, "migrations")
}
