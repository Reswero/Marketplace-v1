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
	g := d.router.Group("/v1/accounts")

	g.POST("/customers", d.CreateCustomer)
	g.POST("/sellers", d.CreateSeller)
	g.POST("/staffs", d.CreateStaff, authorization.Middleware)
	g.POST("/admins", d.CreateAdmin, authorization.Middleware)

	g.GET("/:id", d.GetAccount, authorization.Middleware)
	g.PUT("/:id/password", d.ChangePassword, authorization.Middleware)
	g.PUT("/:id/email", d.ChangeEmail, authorization.Middleware)
}

// @Summary Создание покупателя
// @Description Создание аккаунта покупателя
// @ID create-customer
// @Tags accounts
// @Accept json
// @Produce json
// @Param account body account.CustomerAccountCreateVm true "customer account"
// @Success 201 {object} session.SessionVm
// @Failure 400 {object} StatusResponse
// @Failure 500 {object} StatusResponse
// @Router /accounts/customers [post]
func (d *Delivery) CreateCustomer(c echo.Context) error {
	const op = "delivery.account.CreateCustomer"
	ctx := c.Request().Context()

	var vm *account.CustomerAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	if err = c.Validate(vm); err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, err.Error()))
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

// @Summary Создание продавца
// @Description Создание аккаунта продавца
// @ID create-seller
// @Tags accounts
// @Accept json
// @Produce json
// @Param account body account.SellerAccountCreateVm true "seller account"
// @Success 201 {object} session.SessionVm
// @Failure 400 {object} StatusResponse
// @Failure 500 {object} StatusResponse
// @Router /accounts/sellers [post]
func (d *Delivery) CreateSeller(c echo.Context) error {
	const op = "delivery.account.CreateSeller"
	ctx := c.Request().Context()

	var vm *account.SellerAccountCreateVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	if err = c.Validate(vm); err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, err.Error()))
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

// @Summary Создание персонала
// @Description Создание аккаунта персонала
// @Security AccountId
// @Security AccountType
// @ID create-staff
// @Tags accounts
// @Accept json
// @Produce json
// @Param account body account.StaffAccountCreateVm true "staff account"
// @Success 201 {object} account.AccountCreatedVm
// @Failure 400 {object} StatusResponse
// @Failure 500 {object} StatusResponse
// @Router /accounts/staffs [post]
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

	if err = c.Validate(vm); err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, err.Error()))
	}

	dto := account.MapToStaffAccountDto(vm)

	id, err := d.ucAccount.CreateStaff(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	return c.JSON(http.StatusCreated, &account.AccountCreatedVm{Id: id})
}

// @Summary Создание администратора
// @Description Создание аккаунта администратора
// @Security AccountId
// @Security AccountType
// @ID create-admin
// @Tags accounts
// @Accept json
// @Produce json
// @Param account body account.AdminAccountCreateVm true "admin account"
// @Success 201 {object} account.AccountCreatedVm
// @Failure 400 {object} StatusResponse
// @Failure 500 {object} StatusResponse
// @Router /accounts/admins [post]
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

	if err = c.Validate(vm); err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, err.Error()))
	}

	dto := account.MapToAdminAccountDto(vm)

	id, err := d.ucAccount.CreateAdmin(ctx, dto)
	if err != nil {
		d.logError(op, ErrAccountNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotCreated))
	}

	return c.JSON(http.StatusOK, &account.AccountCreatedVm{Id: id})
}

// @Summary Получение аккаунта
// @Description Получение аккаунта пользователя
// @Security AccountId
// @Security AccountType
// @ID get-account
// @Tags accounts
// @Accept json
// @Produce json
// @Param id path int true "account id"
// @Success 200 {object} account.AccountVm
// @Failure 400 {object} StatusResponse
// @Failure 403
// @Failure 404
// @Failure 500 {object} StatusResponse
// @Router /accounts/{id} [get]
func (d *Delivery) GetAccount(c echo.Context) error {
	const op = "delivery.account.GetAccount"
	ctx := c.Request().Context()

	id, err := getId(c)
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
			return c.JSON(http.StatusNotFound, NewStatusResp(http.StatusNotFound, ErrAccountNotFound)) // change to no content
		}

		d.logError(op, ErrAccountNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotRetrieved))
	}

	vm := account.MapToAccountVm(acc)

	return c.JSON(http.StatusOK, vm)
}

// @Summary Изменение пароля
// @Description Изменение пароля аккаунта
// @Security AccountId
// @Security AccountType
// @ID change-account-password
// @Tags accounts
// @Accept json
// @Produce json
// @Param id path int true "account id"
// @Success 200
// @Failure 400 {object} StatusResponse
// @Failure 403
// @Failure 500 {object} StatusResponse
// @Router /accounts/{id}/password [put]
func (d *Delivery) ChangePassword(c echo.Context) error {
	const op = "delivery.account.ChangePassword"
	ctx := c.Request().Context()

	id, err := getId(c)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidParam))
	}

	claims := authorization.GetClaims(c)
	if claims.AccountId != id {
		return c.NoContent(http.StatusForbidden)
	}

	var vm *account.ChangePasswordVm
	err = c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	if err = c.Validate(vm); err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, err.Error()))
	}

	dto := account.MapToChangePasswordDto(id, vm)

	ok, err := d.ucAccount.ChangePassword(ctx, dto)
	if err != nil {
		d.logError(op, ErrPasswordNotChanged, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrPasswordNotChanged))
	}

	if !ok {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrWrongPassword))
	}

	return c.NoContent(http.StatusOK)
}

// @Summary Изменение почты
// @Description Изменение почты аккаунта
// @Security AccountId
// @Security AccountType
// @ID change-account-email
// @Tags accounts
// @Accept json
// @Produce json
// @Param id path int true "account id"
// @Success 200
// @Failure 400 {object} StatusResponse
// @Failure 403
// @Failure 500 {object} StatusResponse
// @Router /accounts/{id}/email [put]
func (d *Delivery) ChangeEmail(c echo.Context) error {
	const op = "delivery.account.ChangeEmail"
	ctx := c.Request().Context()

	id, err := getId(c)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidParam))
	}

	claims := authorization.GetClaims(c)
	if claims.AccountId != id {
		return c.NoContent(http.StatusForbidden)
	}

	var vm *account.ChangeEmailVm
	err = c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	if err = c.Validate(vm); err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, err.Error()))
	}

	dto := account.MapToChangeEmailDto(id, vm)

	ok, err := d.ucAccount.ChangeEmail(ctx, dto)
	if err != nil {
		d.logError(op, ErrEmailNotChanged, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrEmailNotChanged))
	}

	if !ok {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrWrongPassword))
	}

	return c.NoContent(http.StatusOK)
}

func getId(c echo.Context) (int, error) {
	idParam := c.Param("id")
	return strconv.Atoi(idParam)
}
