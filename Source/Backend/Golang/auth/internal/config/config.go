package config

import (
	"os"

	"github.com/ilyakaznacheev/cleanenv"
)

type Config struct {
	Environment string `yaml:"env"`
	Db
	Cache
	HttpServer `yaml:"http_server"`
	Users
}

type Db struct {
	ConnString string `yaml:"connection_string"`
}

type Cache struct {
	Address string `yaml:"address"`
}

type HttpServer struct {
	Address string `yaml:"address"`
}

type Users struct {
	Address string `yaml:"address"`
	Timeout int    `yaml:"timeout_ms"`
}

func MustLoad() *Config {
	path := os.Getenv("CONFIG_PATH")
	if path == "" {
		panic("CONFIG_PATH is not set")
	}

	if _, err := os.Stat(path); os.IsNotExist(err) {
		panic("config file does not exist")
	}

	cfg := new(Config)
	if err := cleanenv.ReadConfig(path, cfg); err != nil {
		panic(err)
	}

	return cfg
}
