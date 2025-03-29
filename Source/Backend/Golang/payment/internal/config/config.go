package config

import (
	"fmt"
	"os"

	"github.com/ilyakaznacheev/cleanenv"
)

type Config struct {
	Grpc `yaml:"grpc_server"`
}

type Grpc struct {
	Address string `yaml:"address"`
}

func MustLoad() *Config {
	path := os.Getenv("CONFIG_PATH")
	if path == "" {
		panic("CONFIG_PATH is not setted")
	}

	if _, err := os.Stat(path); err != nil {
		panic(err)
	}

	cfg := &Config{}
	if err := cleanenv.ReadConfig(path, *cfg); err != nil {
		panic(fmt.Errorf("error while reading config. %w", err))
	}

	return cfg
}
