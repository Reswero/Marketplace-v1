package main

import (
	"log/slog"
	"os"

	"github.com/Reswero/Marketplace-v1/favorites/internal/config"
	"github.com/Reswero/Marketplace-v1/favorites/internal/delivery/http"
	favoritesRepository "github.com/Reswero/Marketplace-v1/favorites/internal/repository/favorites"
	favoritesUsecase "github.com/Reswero/Marketplace-v1/favorites/internal/usecase/favorites"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

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

	cache, err := redis.New(cfg.Cache.Address)
	if err != nil {
		logger.Error("failed to create redis connection", slog.String("error", err.Error()))
		panic(err)
	}
	defer cache.Close()

	favRepo := favoritesRepository.New(storage)
	ucFavorites := favoritesUsecase.New(favRepo)

	d := http.New(logger, cfg.Environment, ucFavorites)
	if err = d.Start(cfg.HttpServer.Address); err != nil {
		logger.Error("failed while running http server", slog.String("error", err.Error()))
		panic(err)
	}
}
