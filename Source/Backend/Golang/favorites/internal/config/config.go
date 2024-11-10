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
}

type Db struct {
	IpAddress string `yaml:"ip_address"`
	Port      int    `yaml:"port"`
	User      string `yaml:"user"`
	Password  string `yaml:"password"`
	DbName    string `yaml:"db_name"`
}

type Cache struct {
	Address string `yaml:"address"`
}

type HttpServer struct {
	Address string `yaml:"address"`
}

func MustLoad() *Config {
	path := os.Getenv("CONFIG_PATH")
	if path == "" {
		panic("CONFIG_PATH is not set")
	}

	if _, err := os.Stat(path); err != nil {
		panic("config file does not exists")
	}

	cfg := &Config{}
	if err := cleanenv.ReadConfig(path, &cfg); err != nil {
		panic(err)
	}

	return cfg
}
