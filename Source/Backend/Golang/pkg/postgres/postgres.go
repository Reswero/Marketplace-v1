package postgres

import (
	"context"
	"time"

	"github.com/Masterminds/squirrel"
	"github.com/jackc/pgx/v5/pgxpool"
)

type Storage struct {
	Pool    *pgxpool.Pool
	Builder squirrel.StatementBuilderType
}

func New(connString string) (*Storage, error) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second*30)
	defer cancel()

	pool, err := pgxpool.New(ctx, connString)
	if err != nil {
		return nil, err
	}

	if err = pool.Ping(ctx); err != nil {
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
