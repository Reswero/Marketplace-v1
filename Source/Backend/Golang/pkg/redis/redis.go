package redis

import (
	"context"

	"github.com/redis/go-redis/v9"
)

type Cache struct {
	Client *redis.Client
}

func New(address string) (*Cache, error) {
	client := redis.NewClient(&redis.Options{
		Addr:     address,
		Password: "",
		DB:       0,
	})

	if _, err := client.Ping(context.TODO()).Result(); err != nil {
		return nil, err
	}

	return &Cache{
		Client: client,
	}, nil
}

func (c *Cache) Close() error {
	return c.Client.Close()
}
