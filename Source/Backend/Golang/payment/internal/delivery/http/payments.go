package http

import (
	"net/http"

	"github.com/Reswero/Marketplace-v1/payment/internal/delivery/http/payments"
	"github.com/Reswero/Marketplace-v1/pkg/authorization"
	responses "github.com/Reswero/Marketplace-v1/pkg/repsonses"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddPaymentRoutes() {
	g := d.router.Group("v1/payments")

	g.POST("", d.ConfirmPayment, authorization.Middleware)
}

func (d *Delivery) ConfirmPayment(c echo.Context) error {
	const op = "delivery.http.ConfirmPayment"
	ctx := c.Request().Context()

	var vm *payments.PaymentVM
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidRequestBody))
	}

	err = d.ucPayments.ConfirmPayment(ctx, vm.Id)
	if err != nil {
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrConfirmPayment))
	}

	return c.NoContent(http.StatusOK)
}
