package products

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"
	"net/url"
	"strconv"
	"time"

	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

var (
	ErrCheckExistence = errors.New("failed to check products existence")
)

// Взаимодействие с API сервиса Products
type Service struct {
	client  *http.Client
	address string
}

func New(timeout int, address string) *Service {
	return &Service{
		client: &http.Client{
			Timeout: time.Duration(timeout) * time.Millisecond,
		},
		address: address,
	}
}

// Проверка существования товаров по их идентификаторам
func (s *Service) CheckProductsExistence(ctx context.Context, ids []int) ([]int, error) {
	const op = "products.CheckProductsExistence"

	queryParams := url.Values{}
	for _, id := range ids {
		queryParams.Add("ids", strconv.Itoa(id))
	}

	url := fmt.Sprintf("%s/internal/v1/products/existence?%s", s.address, queryParams.Encode())

	resp, err := s.client.Get(url)
	if err != nil {
		return nil, formatter.FmtError(op, err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, formatter.FmtError(op, ErrCheckExistence)
	}

	decoder := json.NewDecoder(resp.Body)
	var existingProducts *ExistingProducts
	err = decoder.Decode(&existingProducts)
	if err != nil {
		return nil, formatter.FmtError(op, ErrCheckExistence)
	}

	return existingProducts.Ids, nil
}
