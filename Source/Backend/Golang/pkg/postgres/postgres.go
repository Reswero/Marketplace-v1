package postgres

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"time"

	"github.com/Masterminds/squirrel"
	"github.com/jackc/pgx/v5/pgxpool"
)

type Config struct {
	IpAddress string
	Port      int
	User      string
	Password  string
	DbName    string
}

type Storage struct {
	Pool    *pgxpool.Pool
	Builder squirrel.StatementBuilderType
}

func New(c *Config) (*Storage, error) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second*30)
	defer cancel()

	err := createDatabase(ctx, c)
	if err != nil {
		return nil, err
	}

	pool, err := connectToDb(ctx, c)
	if err != nil {
		return nil, err
	}

	builder := squirrel.StatementBuilder.PlaceholderFormat(squirrel.Dollar)

	return &Storage{
		Pool:    pool,
		Builder: builder,
	}, nil
}

func (s *Storage) Close() {
	s.Pool.Close()
}

func createDatabase(ctx context.Context, c *Config) error {
	connString := fmt.Sprintf("postgres://%s:%s@%s:%d?sslmode=disable", c.User, c.Password, c.IpAddress, c.Port)

	pool, err := getPool(ctx, connString)
	if err != nil {
		return err
	}
	defer pool.Close()

	var datname string
	err = pool.QueryRow(ctx, "SELECT datname FROM pg_catalog.pg_database WHERE datname = $1", c.DbName).Scan(&datname)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			_, err = pool.Exec(ctx, fmt.Sprintf("CREATE DATABASE \"%s\"", c.DbName))
			if err != nil {
				return err
			}
		} else {
			return err
		}

	}

	return nil
}

func connectToDb(ctx context.Context, c *Config) (*pgxpool.Pool, error) {
	connString := fmt.Sprintf("postgres://%s:%s@%s:%d/%s?sslmode=disable", c.User, c.Password, c.IpAddress, c.Port, c.DbName)
	return getPool(ctx, connString)
}

func getPool(ctx context.Context, connString string) (*pgxpool.Pool, error) {
	pool, err := pgxpool.New(ctx, connString)
	if err != nil {
		return nil, err
	}

	if err = pool.Ping(ctx); err != nil {
		return nil, err
	}

	return pool, nil
}
