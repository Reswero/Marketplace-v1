package main

import (
	"embed"
	"log/slog"
	"os"

	"github.com/Reswero/Marketplace-v1/payment/internal/config"
	"github.com/Reswero/Marketplace-v1/payment/internal/delivery/grpc"
	"github.com/Reswero/Marketplace-v1/payment/internal/delivery/http"
	"github.com/Reswero/Marketplace-v1/payment/internal/repository/payments"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
	"github.com/jackc/pgx/v5/stdlib"
	"github.com/pressly/goose/v3"

	usecase "github.com/Reswero/Marketplace-v1/payment/internal/usecase/payments"
)

//go:embed migrations
var embedMigrations embed.FS

// @title Payment Service
// @version 1.0
// @description Сервис оплаты заказов

// @host localhost:8096
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
		logger.Error("failed to migrate database", slog.String("error", err.Error()))
		panic(err)
	}

	paymentsRepo := payments.New(storage)
	ucPayments := usecase.New(paymentsRepo)

	go func() {
		httpDelivey := http.New(logger, cfg.Env, ucPayments)
		if err = httpDelivey.Start(cfg.Http.Address); err != nil {
			logger.Error("failed while running http server", slog.String("error", err.Error()))
			panic(err)
		}
	}()

	grpcDelivery := grpc.New(logger, cfg.Env, ucPayments)
	if err = grpcDelivery.Start(cfg.Grpc.Address); err != nil {
		logger.Error("failed while running grpc server", slog.String("error", err.Error()))
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
