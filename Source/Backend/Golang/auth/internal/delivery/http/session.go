package http

import (
	"errors"
	"net/http"
	"strconv"

	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http/session"
	sessionPkg "github.com/Reswero/Marketplace-v1/auth/internal/pkg/session"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/authorization"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddSessionRoutes() {
	g := d.router.Group("/v1")

	g.GET("/", d.Authorize)
	g.POST("/login", d.Login)
	g.GET("/logout", d.Logout)
}

// @Summary Авторизация
// @Description Помещение авторизационных данных в заголовки ответа
// @Security SessionId
// @ID sessions-authorize
// @Tags sessions
// @Accept json
// @Produce json
// @Header 200 {string} X-Account-Id "account id"
// @Header 200 {string} X-Account-Type "account type"
// @Success 200
// @Failure 401 {object} StatusResponse
// @Failure 500 {object} StatusResponse
// @Router / [get]
func (d *Delivery) Authorize(c echo.Context) error {
	const op = "delivery.http.Authorize"
	ctx := c.Request().Context()

	auth, ok := c.Request().Header[authorization.AuthorizationHeader]
	if !ok || len(auth) == 0 {
		return c.NoContent(http.StatusUnauthorized)
	}

	sessionId := auth[0]
	session, err := d.ucSession.Get(ctx, sessionId)
	if err != nil {
		if errors.Is(err, sessionPkg.ErrSessionNotFound) {
			return c.JSON(http.StatusUnauthorized, NewStatusResp(http.StatusUnauthorized, ErrSessionExpired))
		}

		d.logError(op, ErrSessionNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrSessionNotRetrieved))
	}

	accId := strconv.Itoa(session.AccountId)
	c.Response().Header().Set(authorization.AccountIdHeader, accId)
	c.Response().Header().Set(authorization.AccountTypeHeader, string(session.AccountType))

	return c.NoContent(http.StatusOK)
}

// @Summary Вход в аккаунт
// @Description Создание пользовательской сессии
// @ID sessions-login
// @Tags sessions
// @Accept json
// @Produce json
// @Param credentials body session.CredentialsVm true "account credentials"
// @Success 200 {object} session.SessionVm
// @Failure 400 {object} StatusResponse
// @Failure 500 {object} StatusResponse
// @Router /login [post]
func (d *Delivery) Login(c echo.Context) error {
	const op = "delivery.http.Login"
	ctx := c.Request().Context()

	var vm *session.CredentialsVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrInvalidRequestBody))
	}

	dto := session.MapToCredentialsDto(vm)

	acc, valid, err := d.ucAccount.ValidateCredentials(ctx, dto)
	if err != nil {
		if !errors.Is(err, usecase.ErrAccountNotFound) {
			d.logError(op, ErrWrongPhoneOrPassword, err)
			return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrAccountNotRetrieved))
		}

		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrWrongPhoneOrPassword))
	}

	if !valid {
		return c.JSON(http.StatusBadRequest, NewStatusResp(http.StatusBadRequest, ErrWrongPhoneOrPassword))
	}

	sessionId, err := d.ucSession.Create(ctx, acc)
	if err != nil {
		d.logError(op, ErrSessionNotCreated, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrSessionNotCreated))
	}

	return c.JSON(http.StatusOK, &session.SessionVm{Id: sessionId})
}

// @Summary Выход из аккаунта
// @Description Завершение пользовательской сессии
// @Security SessionId
// @ID sessions-logout
// @Tags sessions
// @Accept json
// @Produce json
// @Success 200
// @Failure 401
// @Failure 500 {object} StatusResponse
// @Router /logout [get]
func (d *Delivery) Logout(c echo.Context) error {
	const op = "delivery.http.Logout"
	ctx := c.Request().Context()

	auth, ok := c.Request().Header[authorization.AuthorizationHeader]
	if !ok {
		return c.NoContent(http.StatusUnauthorized)
	}

	sessionId := auth[0]
	err := d.ucSession.Delete(ctx, sessionId)
	if err != nil {
		d.logError(op, ErrSessionNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, NewStatusResp(http.StatusInternalServerError, ErrSessionNotRetrieved))
	}

	return c.NoContent(http.StatusOK)
}
