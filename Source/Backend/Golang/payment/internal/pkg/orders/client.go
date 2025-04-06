package orders

import (
	"context"

	pb "github.com/Reswero/Marketplace-v1/payment/internal/pkg/orders/orders_grpc"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

type Client struct {
	conn   *grpc.ClientConn
	client pb.OrdersServiceClient
}

func NewClient(address string) (*Client, error) {
	conn, err := grpc.NewClient(address, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		return nil, err
	}

	c := pb.NewOrdersServiceClient(conn)
	return &Client{
		conn:   conn,
		client: c,
	}, nil
}

func (c *Client) Close() error {
	return c.conn.Close()
}

func (c *Client) ConfirmPayment(ctx context.Context, orderId int64) error {
	const op = "pkg.orders.ConfirmOrderPayment"

	req := &pb.ConfirmOrderPaymentRequest{
		OrderId: orderId,
	}

	_, err := c.client.ConfirmOrderPayment(ctx, req)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}
