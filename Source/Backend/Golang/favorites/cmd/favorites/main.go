package main

import (
	"embed"
	"log/slog"
	"os"

	"github.com/Reswero/Marketplace-v1/favorites/internal/config"
	"github.com/Reswero/Marketplace-v1/favorites/internal/delivery/http"
	favoritesRepository "github.com/Reswero/Marketplace-v1/favorites/internal/repository/favorites"
	favoritesUsecase "github.com/Reswero/Marketplace-v1/favorites/internal/usecase/favorites"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
	"github.com/jackc/pgx/v5/stdlib"
	"github.com/pressly/goose/v3"
)

//go:embed migrations
var embedMigrations embed.FS

// @title Favorites Service
// @version 1.0
// @description Сервис списков избранных товаров

// @host localhost:8088
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
		logger.Error("failed to migrate db", slog.String("error", err.Error()))
		panic(err)
	}

	// cache, err := redis.New(cfg.Cache.Address)
	// if err != nil {
	// 	logger.Error("failed to create redis connection", slog.String("error", err.Error()))
	// 	panic(err)
	// }
	// defer cache.Close()

	favRepo := favoritesRepository.New(storage)
	ucFavorites := favoritesUsecase.New(favRepo)

	d := http.New(logger, cfg.Environment, ucFavorites)
	if err = d.Start(cfg.HttpServer.Address); err != nil {
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
