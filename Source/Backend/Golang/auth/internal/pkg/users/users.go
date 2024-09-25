package users

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"
	"time"

	"github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/customer"
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/seller"
	"github.com/Reswero/Marketplace-v1/auth/internal/domain/staff"
	"github.com/Reswero/Marketplace-v1/pkg/formatter"
)

var (
	ErrCreateAdminFailed = errors.New("failed to create administrator")
)

// Взаимодействие с API сервиса Users
type Users struct {
	Client         *http.Client
	ServiceAddress string
}

func New(serviceAddress string) *Users {
	return &Users{
		Client: &http.Client{
			Timeout: 10 * time.Second,
		},
		ServiceAddress: serviceAddress,
	}
}

// Создание профиля администратора
func (u *Users) CreateAdministrator(ctx context.Context, acc *account.Account) error {
	const op = "users.CreateAdministrator"

	url := fmt.Sprintf("%s/internal/v1/administrators", u.ServiceAddress)

	body, err := json.Marshal(acc)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.doCreateRequest(ctx, url, body)
	if err != nil {
		return err
	}

	return nil
}

// Создание профиля покупателя
func (u *Users) CreateCustomer(ctx context.Context, cus *customer.Customer) error {
	const op = "users.CreateCustomer"

	url := fmt.Sprintf("%s/internal/v1/customers", u.ServiceAddress)

	body, err := json.Marshal(cus)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.doCreateRequest(ctx, url, body)
	if err != nil {
		return err
	}

	return nil
}

// Создание профиля продавца
func (u *Users) CreateSeller(ctx context.Context, sel *seller.Seller) error {
	const op = "users.CreateSeller"

	url := fmt.Sprintf("%s/internal/v1/sellers", u.ServiceAddress)

	body, err := json.Marshal(sel)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.doCreateRequest(ctx, url, body)
	if err != nil {
		return err
	}

	return nil
}

// Создание профиля персонала
func (u *Users) CreateStaff(ctx context.Context, acc *staff.Staff) error {
	const op = "users.CreateStaff"

	url := fmt.Sprintf("%s/internal/v1/staffs", u.ServiceAddress)

	body, err := json.Marshal(acc)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	err = u.doCreateRequest(ctx, url, body)
	if err != nil {
		return err
	}

	return nil
}

func (u *Users) doCreateRequest(ctx context.Context, url string, body []byte) error {
	const op = "users.doCreateRequest"

	reader := bytes.NewReader(body)

	req, err := http.NewRequest(http.MethodPost, url, reader)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	resp, err := u.Client.Do(req)
	if err != nil {
		return formatter.FmtError(op, err)
	}

	if resp.StatusCode != http.StatusCreated {
		return formatter.FmtError(op, ErrCreateAdminFailed)
	}

	return nil
}
