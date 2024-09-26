package outbox

import (
	"database/sql"
	"encoding/json"

	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	_ "github.com/mattn/go-sqlite3"
)

const (
	driver = "sqlite3"
	create = `
	CREATE TABLE IF NOT EXISTS outbox(
		id INTEGER PRIMARY KEY AUTOINCREMENT,
		value TEXT NOT NULL
	);
	`
)

type DbQueue[T any] struct {
	name string
	db   *sql.DB
}

func NewDbQueue[T any](name string) (*DbQueue[T], error) {
	db, err := sql.Open(driver, name)
	if err != nil {
		return nil, err
	}

	_, err = db.Exec(create)
	if err != nil {
		return nil, err
	}

	return &DbQueue[T]{
		name: name,
		db:   db,
	}, nil
}

func (o *DbQueue[T]) Close() error {
	return o.db.Close()
}

func (o *DbQueue[T]) Push(value *T) error {
	const op = "outbox.DbQueue.Push"

	bytes, err := json.Marshal(value)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	_, err = o.db.Exec("INSERT INTO outbox(value) VALUES(?)", bytes)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (o *DbQueue[T]) Peek() (T, error) {
	const op = "outbox.DbQueue.Peek"

	rows, err := o.db.Query("SELECT value FROM outbox LIMIT 1")
	if err != nil {
		return *new(T), formatter.FmtError(op, err)
	}
	defer rows.Close()

	var bytes []byte
	if rows.Next() {
		err = rows.Scan(&bytes)
		if err != nil {
			return *new(T), formatter.FmtError(op, err)
		}
	} else {
		return *new(T), sql.ErrNoRows
	}

	var value T
	err = json.Unmarshal(bytes, &value)
	if err != nil {
		return *new(T), formatter.FmtError(op, err)
	}

	return value, nil
}

func (o *DbQueue[T]) Pop() error {
	const op = "outbox.DbQueue.Pop"

	_, err := o.db.Exec("DELETE FROM outbox WHERE id = (SELECT id FROM outbox LIMIT 1)")
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
