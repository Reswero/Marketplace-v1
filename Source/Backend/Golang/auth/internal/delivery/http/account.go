package http

import (
	"errors"
	"net/http"
	"strconv"

	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http/session"
	domain "github.com/Reswero/Marketplace-v1/auth/internal/domain/account"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/authorization"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddAccountRoutes() {
	g := d.router.Group("/accounts")

	g.POST("/customers", d.CreateCustomer)
	g.POST("/sellers", d.CreateSeller)
	g.POST("/staffs", d.CreateStaff, authorization.Middleware)
	g.POST("/admins", d.CreateAdmin, authorization.Middleware)

	g.GET("/:id", d.GetAccount, authorization.Middleware)
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

	acc, err := d.ucAccount.Get(ctx, id)
	if err != nil {
		d.logError(op, ErrAccountNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotRetrieved))
	}

	sessionId, err := d.ucSession.Create(ctx, acc)
	if err != nil {
		d.logError(op, ErrSessionNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrSessionNotCreated))
	}

	return c.JSON(http.StatusCreated, &session.SessionVm{Id: sessionId})
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

	acc, err := d.ucAccount.Get(ctx, id)
	if err != nil {
		d.logError(op, ErrAccountNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotRetrieved))
	}

	sessionId, err := d.ucSession.Create(ctx, acc)
	if err != nil {
		d.logError(op, ErrSessionNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrSessionNotCreated))
	}

	return c.JSON(http.StatusCreated, &session.SessionVm{Id: sessionId})
}

func (d *Delivery) CreateStaff(c echo.Context) error {
	const op = "delivery.account.CreateStaff"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)
	if claims.AccountType != string(domain.Administrator) {
		return c.NoContent(http.StatusForbidden)
	}

	var vm *account.StaffAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

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

	claims := authorization.GetClaims(c)
	if claims.AccountType != string(domain.Administrator) {
		return c.NoContent(http.StatusForbidden)
	}

	var vm *account.AdminAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	dto := account.MapToAdminAccountDto(vm)

	id, err := d.ucAccount.CreateAdmin(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	return c.JSON(http.StatusOK, &account.AccountCreatedVm{Id: id})
}

func (d *Delivery) GetAccount(c echo.Context) error {
	const op = "delivery.account.GetAccount"
	ctx := c.Request().Context()

	idParam := c.Param("id")
	id, err := strconv.Atoi(idParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidParam))
	}

	claims := authorization.GetClaims(c)
	if claims.AccountId != id && claims.AccountType != string(domain.Administrator) {
		return c.NoContent(http.StatusForbidden)
	}

	acc, err := d.ucAccount.Get(ctx, id)
	if err != nil {
		if errors.Is(err, usecase.ErrAccountNotFound) {
			return c.JSON(http.StatusNotFound, NewStatusResp(http.StatusNotFound, ErrAccountNotFound))
		}

		d.logError(op, ErrAccountNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotRetrieved))
	}

	vm := account.MapToAccountVm(acc)

	return c.JSON(http.StatusOK, vm)
}
