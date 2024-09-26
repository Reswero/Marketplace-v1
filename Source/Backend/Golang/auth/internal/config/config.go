package config

import (
	"fmt"
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

type Users struct {
	Address string `yaml:"address"`
	Timeout int    `yaml:"timeout_ms"`
}

func MustLoad() *Config {
	path := os.Getenv("CONFIG_PATH")
	fmt.Println(path)
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
