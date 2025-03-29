package grpc

import (
	"log/slog"
	"net"

	pb "github.com/Reswero/Marketplace-v1/payment/internal/delivery/grpc/payment_grpc"
	"github.com/Reswero/Marketplace-v1/payment/internal/usecase"
	grpc "google.golang.org/grpc"
)

type Delivery struct {
	logger     *slog.Logger
	ucPayments usecase.Payments
}

func New(logger *slog.Logger, env string, ucPayments usecase.Payments) *Delivery {
	return &Delivery{
		logger:     logger,
		ucPayments: ucPayments,
	}
}

func (d *Delivery) Start(address string) error {
	listener, err := net.Listen("tcp", address)
	if err != nil {
		panic(err)
	}

	s := newServer(d.logger, d.ucPayments)
	g := grpc.NewServer()

	pb.RegisterPaymentServiceServer(g, s)

	return g.Serve(listener)
}
