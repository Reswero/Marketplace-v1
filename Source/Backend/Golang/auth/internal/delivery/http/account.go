package http

import (
	"fmt"
	"net/http"

	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http/account"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddAccountRoutes() {
	g := d.router.Group("/accounts")

	g.POST("/customers", d.CreateCustomer)
}

func (d *Delivery) CreateCustomer(c echo.Context) error {
	const op = "delivery.account.CreateCustomer"
	ctx := c.Request().Context()

	var vm *account.CustomerAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	dto := account.MapToCustomerAccountDto(vm)

	id, err := d.ucAccount.CreateCustomer(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	// Create session

	return c.String(http.StatusOK, fmt.Sprintf("%d", id)) // Return session id
}

func (d *Delivery) CreateSeller(c echo.Context) error {
	const op = "delivery.account.CreateSeller"
	ctx := c.Request().Context()

	var vm *account.SellerAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	dto := account.MapToSellerAccountDto(vm)

	id, err := d.ucAccount.CreateSeller(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	// Create session

	return c.String(http.StatusOK, fmt.Sprintf("%d", id)) // Return session id
}

func (d *Delivery) CreateStaff(c echo.Context) error {
	const op = "delivery.account.CreateStaff"
	ctx := c.Request().Context()

	var vm *account.StaffAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	// Check user claims. Only admin can create staff

	dto := account.MapToStaffAccountDto(vm)

	id, err := d.ucAccount.CreateStaff(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	return c.JSON(http.StatusCreated, &account.AccountCreatedVm{Id: id})
}

func (d *Delivery) CreateAdmin(c echo.Context) error {
	const op = "delivery.account.CreateAdmin"
	ctx := c.Request().Context()

	var vm *account.AdminAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	// Check user claims. Only admin can create admin

	dto := account.MapToAdminAccountDto(vm)

	id, err := d.ucAccount.CreateAdmin(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	return c.JSON(http.StatusOK, &account.AccountCreatedVm{Id: id})
}
