package outbox_test

import (
	"database/sql"
	"errors"
	"os"
	"testing"

	"github.com/Reswero/Marketplace-v1/pkg/outbox"
	_ "github.com/mattn/go-sqlite3"
)

const dbName = "test.db"

type Test struct {
	Value1 int
	Value2 string
}

func TestDbQueuePush(t *testing.T) {
	queue, err := outbox.NewDbQueue[Test](dbName)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	value := &Test{Value1: 1, Value2: "test"}

	err = queue.Push(value)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	cleanUp(t, queue)
}

func TestDbQueuePeek(t *testing.T) {
	queue, err := outbox.NewDbQueue[Test](dbName)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	value := &Test{Value1: 1, Value2: "test"}

	err = queue.Push(value)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	peeked, err := queue.Peek()
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	if value.Value1 != peeked.Value1 || value.Value2 != peeked.Value2 {
		t.Errorf("Error: Expected %v, got %v", value, peeked)
	}

	cleanUp(t, queue)
}

func TestDbQueuePop(t *testing.T) {
	queue, err := outbox.NewDbQueue[Test](dbName)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	value := &Test{Value1: 1, Value2: "test"}

	err = queue.Push(value)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	poped, err := queue.Pop()
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	if value.Value1 != poped.Value1 || value.Value2 != poped.Value2 {
		t.Errorf("Error: Expected %v, got %v", value, poped)
	}

	_, err = queue.Peek()
	if errors.Is(err, sql.ErrNoRows) == false {
		t.Errorf("Error: Excpected no rows")
	}

	cleanUp(t, queue)
}

func cleanUp(t *testing.T, queue *outbox.DbQueue[Test]) {
	err := queue.Close()
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	err = os.Remove(dbName)
	if err != nil {
		t.Errorf("Error: %v", err)
	}
}
