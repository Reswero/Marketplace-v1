package http

import (
	"errors"
	"net/http"

	"github.com/Reswero/Marketplace-v1/auth/internal/delivery/http/session"
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddSessionRoutes() {
	d.router.POST("/login", d.Login)
}

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
