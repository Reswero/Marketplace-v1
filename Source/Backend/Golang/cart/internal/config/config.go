package config

import (
	"os"

	"github.com/ilyakaznacheev/cleanenv"
)

type Config struct {
	Environment string `yaml:"env"`
	Cache
	HttpServer `yaml:"http_server"`
	Products
	Orders
}

type Cache struct {
	Address string `yaml:"address"`
}

type HttpServer struct {
	Address string `yaml:"address"`
}

type Products struct {
	Address string `yaml:"address"`
	Timeout int    `yaml:"timeout_ms"`
}

type Orders struct {
	Address string `yaml:"address"`
	Timeout int    `yaml:"timeout_ms"`
}

func MustLoad() *Config {
	path := os.Getenv("CONFIG_PATH")
	if path == "" {
		panic("CONFIG_PATH is not set")
	}

	if _, err := os.Stat(path); err != nil {
		panic(err)
	}

	cfg := &Config{}
	if err := cleanenv.ReadConfig(path, cfg); err != nil {
		panic("config file does not exists")
	}

	return cfg
}
