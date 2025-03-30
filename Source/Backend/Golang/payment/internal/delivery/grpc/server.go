package grpc

import (
	"context"
	"log/slog"

	pb "github.com/Reswero/Marketplace-v1/payment/internal/delivery/grpc/payment_grpc"
	"github.com/Reswero/Marketplace-v1/payment/internal/domain/order"
	"github.com/Reswero/Marketplace-v1/payment/internal/usecase"
	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/status"
)

type Server struct {
	pb.UnimplementedPaymentServiceServer
	logger     *slog.Logger
	ucPayments usecase.Payments
}

func newServer(logger *slog.Logger, ucPayments usecase.Payments) *Server {
	return &Server{
		logger:     logger,
		ucPayments: ucPayments,
	}
}

func (s *Server) GetLink(ctx context.Context, request *pb.PaymentLinkRequest) (*pb.PaymentLinkResponse, error) {
	const op = "delivery.payment.GetLink"

	order := order.New(request.GetOrderId(), int(request.GetPaybleAmount()))

	link, err := s.ucPayments.CreatePayment(ctx, order)
	if err != nil {
		return nil, status.Error(codes.Unknown, "unknown error")
	}

	return &pb.PaymentLinkResponse{
		Link: link,
	}, nil
}
