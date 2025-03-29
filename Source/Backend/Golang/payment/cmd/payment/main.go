package main

import "github.com/Reswero/Marketplace-v1/payment/internal/config"

func main() {
	cfg := config.MustLoad()
	_ = cfg
}
