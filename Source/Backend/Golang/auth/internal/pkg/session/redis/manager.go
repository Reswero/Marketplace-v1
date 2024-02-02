package redis

import (
	"context"
	"encoding/json"
	"fmt"
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/generator"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/session"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
)

const (
	idLen      = 32
	sessionKey = "session:%s"
)

var (
	sessionDuration = time.Hour * 24
)

type SessionManager struct {
	*redis.Cache
}

func New(cache *redis.Cache) *SessionManager {
	return &SessionManager{
		Cache: cache,
	}
}

func (m *SessionManager) CreateSession(ctx context.Context, acc *account.Account) (string, error) {
	const op = "session.redis.CreateSession"

	id, err := m.generateId(ctx)
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	key := fmt.Sprintf(sessionKey, id)
	session := session.New(acc)

	val, err := json.Marshal(session)
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	_, err = m.Client.Set(ctx, key, val, sessionDuration).Result()
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	return id, nil
}

func (m *SessionManager) generateId(ctx context.Context) (string, error) {
	const op = "session.redis.generateId"

	var id string
	for id == "" {
		id, err := generator.GetRandomAsciiString(idLen)
		if err != nil {
			return "", formatter.FmtError(op, err)
		}

		exists, err := m.checkExists(ctx, id)
		if err != nil {
			return "", formatter.FmtError(op, err)
		}

		if exists {
			id = ""
		}
	}

	return id, nil
}

func (m *SessionManager) checkExists(ctx context.Context, id string) (bool, error) {
	const op = "session.redis.checkExists"

	key := fmt.Sprintf(sessionKey, id)
	exists, err := m.Client.Exists(ctx, key).Result()
	if err != nil {
		return false, formatter.FmtError(op, err)
	}

	if exists != 1 {
		return false, nil
	}

	return true, nil
}
