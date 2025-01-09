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
	p := c.Group("/products")

	c.GET("", d.Get, authorization.Middleware)
	c.DELETE("", d.Clear, authorization.Middleware)
	c.POST("/checkout", d.Checkout, authorization.Middleware)

	p.POST("", d.AddProduct, authorization.Middleware)
	p.DELETE("/:id", d.DeleteProduct, authorization.Middleware)
	p.PUT("/:id/count", d.ChangeProductCount, authorization.Middleware)
}

// @id get-cart
// @summary Получение товаров из корзины
// @description Получение товаров из корзины покупателя
// @router /cart [get]
// @security AccountId
// @security AccountType
// @tags cart
// @accept json
// @produce json
// @success 200 {object} []viewmodel.Product
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
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

// @id clear-cart
// @summary Очистка корзины
// @description Очистка корзины покупателя
// @router /cart [delete]
// @security AccountId
// @security AccountType
// @tags cart
// @accept json
// @produce json
// @success 200
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
func (d *Delivery) Clear(c echo.Context) error {
	const op = "delivery.cart.Clear"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	err := d.ucCart.Clear(ctx, claims.AccountId)
	if err != nil {
		d.logError(op, ErrCartNotCleared, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrCartNotCleared))
	}

	return c.NoContent(http.StatusOK)
}

// @id cart-checkout
// @summary Оформление товаров из корзины
// @description Оформление товаров из корзины покупателя
// @router /cart/checkout [post]
// @security AccountId
// @security AccountType
// @tags cart
// @accept json
// @produce json
// @success 200
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
func (d *Delivery) Checkout(c echo.Context) error {
	const op = "delivery.cart.Checkout"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	err := d.ucCart.Checkout(ctx, claims.AccountId)
	if err != nil {
		d.logError(op, ErrCartNotCheckouted, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrCartNotCheckouted))
	}

	return c.NoContent(http.StatusOK)
}

// @id cart-add-product
// @summary Добавление товара в корзину
// @description Добавление товара в корзину покупателя
// @router /cart/products [post]
// @security AccountId
// @security AccountType
// @tags cart
// @accept json
// @produce json
// @param product body viewmodel.AddProduct true "Товар"
// @success 200
// @failure 400 {object} responses.StatusResponse
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
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

	return c.NoContent(http.StatusOK)
}

// @id cart-delete-product
// @summary Удаление товара из корзины
// @description Удаление товара из корзины покупателя
// @router /cart/products/{id} [delete]
// @security AccountId
// @security AccountType
// @tags cart
// @accept json
// @produce json
// @param id path int true "Идентификатор товара"
// @success 200
// @failure 400 {object} responses.StatusResponse
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
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

// @id cart-change-product-count
// @summary Обновить количество товара в корзине
// @description Обновить количество товара в корзине покупателя
// @router /cart/products/{id}/count [put]
// @security AccountId
// @security AccountType
// @tags cart
// @accept json
// @produce json
// @param id path int true "Идентификатор товара"
// @param count body viewmodel.AddCount true "Количество"
// @success 200
// @failure 400 {object} responses.StatusResponse
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
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

	return c.NoContent(http.StatusOK)
}
