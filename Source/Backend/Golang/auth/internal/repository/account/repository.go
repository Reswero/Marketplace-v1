package account

import (
	"context"
	sqldb "database/sql"
	"errors"
	"time"

	"github.com/Masterminds/squirrel"
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/postgres"
)

const (
	accountsTable = "accounts"
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

	sql, args, err := r.Builder.Insert(accountsTable).
		Columns("type", "phone_number", "email",
			"created_at", "password", "salt").
		Values(acc.Type, acc.PhoneNumber, acc.Email,
			time.Now(), acc.Password, acc.Salt).
		Suffix("RETURNING id").
		ToSql()
	if err != nil {
		return 0, formatter.FmtError(op, err)
	}

	err = r.Pool.QueryRow(ctx, sql, args...).Scan(&acc.Id)
	if err != nil {
		return 0, formatter.FmtError(op, err)
	}

	return acc.Id, nil
}

// Получение аккаунта
func (r *Repository) GetAccount(ctx context.Context, id int) (*account.Account, error) {
	const op = "repository.account.GetAccount"

	sql, args, err := r.Builder.Select("*").
		From(accountsTable).
		Where(squirrel.Eq{"id": id}).
		ToSql()
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	acc, err := r.getAccountFromQuery(ctx, sql, args...)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return acc, nil
}

// Получение аккаунта по номеру телефона и типу
func (r *Repository) GetAccountByPhoneNumber(ctx context.Context, phoneNumber string,
	accType account.AccountType) (*account.Account, error) {
	const op = "repository.account.GetAccountByPhoneNumber"

	sql, args, err := r.Builder.Select("*").
		From(accountsTable).
		Where(squirrel.Eq{"phone_number": phoneNumber, "type": accType}).
		ToSql()
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	acc, err := r.getAccountFromQuery(ctx, sql, args...)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return acc, nil
}

// Обновление аккаунта
func (r *Repository) UpdateAccount(ctx context.Context, acc *account.Account) error {
	const op = "repository.account.UpdateAccount"

	sql, args, err := r.Builder.Update(accountsTable).
		Set("phone_number", acc.PhoneNumber).
		Set("email", acc.Email).
		Set("password", acc.Password).
		Set("salt", acc.Salt).
		Where(squirrel.Eq{"id": acc.Id}).
		ToSql()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Удаление аккаунта
func (r *Repository) DeleteAccount(ctx context.Context, id int) error {
	const op = "repository.account.DeleteAccount"

	sql, args, err := r.Builder.Delete(accountsTable).
		Where(squirrel.Eq{"id": id}).
		ToSql()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_, err = r.Pool.Exec(ctx, sql, args...)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (r *Repository) getAccountFromQuery(ctx context.Context, sql string, args ...interface{}) (*account.Account, error) {
	const op = "repository.account.getAccountFromQuery"

	acc := &account.Account{}
	err := r.Pool.QueryRow(ctx, sql, args...).Scan(&acc.Id, &acc.Type, &acc.PhoneNumber,
		&acc.Email, &acc.CreatedAt, &acc.Password, &acc.Salt)
	if err != nil {
		if errors.Is(err, sqldb.ErrNoRows) {
			return nil, err
		}

		return nil, formatter.FmtError(op, err)
	}

	return acc, nil
}
