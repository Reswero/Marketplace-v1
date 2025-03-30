package config

import (
	"fmt"
	"os"

	"github.com/ilyakaznacheev/cleanenv"
)

type Config struct {
	Env  string `yaml:"env"`
	Grpc `yaml:"grpc_server"`
	Db
}

type Grpc struct {
	Address string `yaml:"address"`
}

type Db struct {
	IpAddress string `yaml:"ip_address"`
	Port      int    `yaml:"port"`
	User      string `yaml:"user"`
	Password  string `yaml:"password"`
	DbName    string `yaml:"db_name"`
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
