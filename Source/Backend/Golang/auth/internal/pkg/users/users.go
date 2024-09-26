package users

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"
	"time"

	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

var (
	ErrCreateFailed = errors.New("failed to create profile")
)

// Взаимодействие с API сервиса Users
type Users struct {
	client  *http.Client
	address string
}

func New(timeout int, address string) *Users {
	fmt.Println(timeout)
	fmt.Println(time.Duration(timeout) * time.Millisecond)

	return &Users{
		client: &http.Client{
			Timeout: time.Duration(timeout) * time.Millisecond,
		},
		address: address,
	}
}

// Создание профиля покупателя
func (u *Users) CreateCustomer(ctx context.Context, dto *CreateCustomerDto) error {
	const op = "users.CreateCustomer"

	url := fmt.Sprintf("%s/internal/v1/customers", u.address)

	err := u.doCreateRequest(ctx, url, dto)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Создание профиля продавца
func (u *Users) CreateSeller(ctx context.Context, dto *CreateSellerDto) error {
	const op = "users.CreateSeller"

	url := fmt.Sprintf("%s/internal/v1/sellers", u.address)

	err := u.doCreateRequest(ctx, url, dto)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Создание профиля персонала
func (u *Users) CreateStaff(ctx context.Context, dto *CreateStaffDto) error {
	const op = "users.CreateStaff"

	url := fmt.Sprintf("%s/internal/v1/staffs", u.address)

	err := u.doCreateRequest(ctx, url, dto)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

// Создание профиля администратора
func (u *Users) CreateAdministrator(ctx context.Context, dto *CreateAdministratorDto) error {
	const op = "users.CreateAdministrator"

	url := fmt.Sprintf("%s/internal/v1/administrators", u.address)

	err := u.doCreateRequest(ctx, url, dto)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	return nil
}

func (u *Users) doCreateRequest(ctx context.Context, url string, dto interface{}) error {
	const op = "users.doCreateRequest"

	body, err := json.Marshal(dto)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	reader := bytes.NewReader(body)

	req, err := http.NewRequest(http.MethodPost, url, reader)
	if err != nil {
		return formatter.FmtError(op, err)
	}
	req.Header.Set("Content-Type", "application/json")

	resp, err := u.client.Do(req)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	fmt.Println(resp.StatusCode)
	if resp.StatusCode != http.StatusCreated {
		return formatter.FmtError(op, ErrCreateFailed)
	}

	return nil
}
