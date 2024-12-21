package http

import (
	"net/http"
	"strconv"

	"github.com/Reswero/Marketplace-v1/cart/internal/delivery/http/mapper"
	"github.com/Reswero/Marketplace-v1/cart/internal/delivery/http/viewmodel"
	"github.com/Reswero/Marketplace-v1/pkg/authorization"
	responses "github.com/Reswero/Marketplace-v1/pkg/repsonses"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddCartRoutes() {
	c := d.router.Group("/v1/cart")
	p := c.Group("products")

	c.GET("", d.Get, authorization.Middleware)
	c.DELETE("", d.Clear, authorization.Middleware)
	c.GET("checkout", d.Checkout, authorization.Middleware)

	p.POST("", d.AddProduct, authorization.Middleware)
	p.DELETE(":id", d.DeleteProduct, authorization.Middleware)
	p.PUT(":id/count", d.ChangeProductCount, authorization.Middleware)
}

func (d *Delivery) Get(c echo.Context) error {
	const op = "delivery.cart.Get"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	products, err := d.ucCart.GetProducts(ctx, claims.AccountId)
	if err != nil {
		d.logError(op, ErrCartNotRetrieved, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrCartNotRetrieved))
	}

	vms := make([]*viewmodel.Product, 0, len(products))
	for _, p := range products {
		vms = append(vms, mapper.MapToProductVm(p))
	}

	return c.JSON(http.StatusOK, vms)
}

func (d *Delivery) Clear(c echo.Context) error {
	const op = "delivery.cart.Clear"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	err := d.ucCart.Clear(ctx, claims.AccountId)
	if err != nil {
		d.logError(op, ErrCartNotCleared, err)
		c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrCartNotCleared))
	}

	return c.NoContent(http.StatusOK)
}

func (d *Delivery) Checkout(c echo.Context) error {
	const op = "delivery.cart.Checkout"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	err := d.ucCart.Checkout(ctx, claims.AccountId)
	if err != nil {
		d.logError(op, ErrCartNotCheckouted, err)
	}

	return c.NoContent(http.StatusOK)
}

func (d *Delivery) AddProduct(c echo.Context) error {
	const op = "delivery.cart.AddProduct"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	var vm *viewmodel.AddProduct
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidRequestBody))
	}

	err = d.ucCart.AddProduct(ctx, claims.AccountId, vm.Id)
	if err != nil {
		d.logError(op, ErrProductNotAdded, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrProductNotAdded))
	}

	return nil
}

func (d *Delivery) DeleteProduct(c echo.Context) error {
	const op = "delivery.cart.DeleteProduct"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	idParam := c.Param("id")
	productId, err := strconv.Atoi(idParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidParam))
	}

	err = d.ucCart.DeleteProduct(ctx, claims.AccountId, productId)
	if err != nil {
		d.logError(op, ErrProductNotDeleted, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrProductNotDeleted))
	}

	return c.NoContent(http.StatusOK)
}

func (d *Delivery) ChangeProductCount(c echo.Context) error {
	const op = "delivery.cart.ChangeProductCount"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	idParam := c.Param("id")
	productId, err := strconv.Atoi(idParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidParam))
	}

	var vm *viewmodel.AddCount
	err = c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidRequestBody))
	}

	err = d.ucCart.ChangeProductCount(ctx, claims.AccountId, productId, vm.Count)
	if err != nil {
		d.logError(op, ErrProductCountNotChanged, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrProductCountNotChanged))
	}

	return nil
}
