package orders

import (
	"bytes"
	"encoding/json"
	"fmt"
	"net/http"
	"time"

	"github.com/Reswero/Marketplace-v1/cart/internal/domain/cart"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

// Клиент к сервису Orders
type Client struct {
	client  *http.Client
	address string
}

func New(timeout int, address string) *Client {
	return &Client{
		client: &http.Client{
			Timeout: time.Duration(timeout) * time.Millisecond,
		},
		address: address,
	}
}

// Создать заказ покупателя
func (c *Client) CreateOrder(products []*cart.Product) error {
	const op = "orders.Client.CreateOrder"

	url := fmt.Sprintf("%s/internal/v1/orders", c.address)
	order := mapToOrder(products)

	body, err := json.Marshal(order)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	reader := bytes.NewReader(body)

	req, err := http.NewRequest(http.MethodPost, url, reader)
	if err != nil {
		return formatter.FmtError(op, err)
	}
	req.Header.Set("Content-Type", "application/json")

	resp, err := c.client.Do(req)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	if resp.StatusCode != http.StatusCreated {
		return formatter.FmtError(op, errCreateFailed)
	}

	return nil
}

func mapToOrder(products []*cart.Product) *orderDto {
	var customerId int
	productsDto := make([]*productDto, 0, len(products))

	for _, p := range products {
		customerId = p.CustomerId

		dto := &productDto{
			Id:       p.ProductId,
			Quantity: p.Count,
		}
		productsDto = append(productsDto, dto)
	}

	return &orderDto{
		CustomerId: customerId,
		Products:   productsDto,
	}
}
