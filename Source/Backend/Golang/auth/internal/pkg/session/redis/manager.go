package redis

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/generator"
	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/session"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"github.com/Reswero/Marketplace-v1/pkg/redis"
	redisdb "github.com/redis/go-redis/v9"
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
	sess := session.New(acc)

	val, err := json.Marshal(sess)
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	_, err = m.Client.Set(ctx, key, val, sessionDuration).Result()
	if err != nil {
		return "", formatter.FmtError(op, err)
	}

	return id, nil
}

func (m *SessionManager) GetSession(ctx context.Context, id string) (*session.Session, error) {
	const op = "session.redis.GetSession"

	key := fmt.Sprintf(sessionKey, id)

	val, err := m.Client.Get(ctx, key).Result()
	if err != nil {
		if errors.Is(err, redisdb.Nil) {
			return nil, session.ErrSessionNotFound
		}

		return nil, formatter.FmtError(op, err)
	}

	sess := &session.Session{}
	err = json.Unmarshal([]byte(val), &sess)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}

	return sess, nil
}

func (m *SessionManager) DeleteSession(ctx context.Context, id string) error {
	const op = "session.redis.DeleteSession"

	key := fmt.Sprintf(sessionKey, id)

	_, err := m.Client.Del(ctx, key).Result()
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (m *SessionManager) generateId(ctx context.Context) (id string, err error) {
	const op = "session.redis.generateId"

	for id == "" {
		id, err = generator.GetRandomAsciiString(idLen)
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
