package account

import (
	"context"
	sqldb "database/sql"
	"errors"
	"fmt"
	"time"

	"github.com/Masterminds/squirrel"
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
)

// Репозиторий аккаунтов
type Repository struct {
	*postgres.Storage
}

func New(storage *postgres.Storage) *Repository {
	return &Repository{
		Storage: storage,
	}
}

// Создание аккаунта
func (r *Repository) CreateAccount(ctx context.Context, acc *account.Account) (int, error) {
	const op = "repository.account.CreateAccount"

	sql, args, err := r.Builder.Insert("accounts").
		Columns("type", "phone_number", "email",
			"created_at", "password", "salt").
		Values(acc.Type, acc.PhoneNumber, acc.Email,
			time.Now(), acc.Password, acc.Salt).
		Suffix(`RETURNING "Id"`).
		ToSql()
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	err = r.Pool.QueryRow(ctx, sql, args...).Scan(&acc.Id)
	if err != nil {
		return 0, fmt.Errorf("%s: %w", op, err)
	}

	return acc.Id, nil
}

// Получение аккаунта
func (r *Repository) GetAccount(ctx context.Context, id int) (*account.Account, error) {
	const op = "repository.account.GetAccount"

	sql, args, err := r.Builder.Select("*").
		From("accounts").
		Where(squirrel.Eq{"id": id}).
		ToSql()
	if err != nil {
		return nil, fmt.Errorf("%s: %w", op, err)
	}

	acc := &account.Account{}
	err = r.Pool.QueryRow(ctx, sql, args...).
		Scan(&acc.Id, &acc.Type, &acc.PhoneNumber,
			&acc.Email, &acc.CreatedAt, &acc.Password, &acc.Salt)
	if err != nil {
		if errors.Is(err, sqldb.ErrNoRows) {
			return nil, err
		}
		return nil, fmt.Errorf("%s: %w", op, err)
	}

	return acc, nil
}

// Обновление аккаунта
func (r *Repository) UpdateAccount(ctx context.Context, acc *account.Account) error {
	const op = "repository.account.UpdateAccount"

	sql, args, err := r.Builder.Update("accounts").
		Set("phone_number", acc.PhoneNumber).
		Set("email", acc.Email).
		Set("password", acc.Password).
		Set("salt", acc.Salt).
		Where(squirrel.Eq{"id": acc.Id}).
		ToSql()
	if err != nil {
		return fmt.Errorf("%s: %w", op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return fmt.Errorf("%s: %w", op, err)
	}

	return nil
}

// Удаление аккаунта
func (r *Repository) DeleteAccount(ctx context.Context, acc *account.Account) error {
	const op = "repository.account.DeleteAccount"

	sql, args, err := r.Builder.Delete("accounts").
		Where(squirrel.Eq{"id": acc.Id}).
		ToSql()
	if err != nil {
		return fmt.Errorf("%s: %w", op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return fmt.Errorf("%s: %w", op, err)
	}

	return nil
}