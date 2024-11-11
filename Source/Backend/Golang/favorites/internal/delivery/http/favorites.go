package http

import (
	"net/http"
	"strconv"

	"github.com/Reswero/Marketplace-v1/favorites/internal/delivery/http/favorites"
	"github.com/Reswero/Marketplace-v1/favorites/internal/usecase"
	"github.com/Reswero/Marketplace-v1/pkg/authorization"
	responses "github.com/Reswero/Marketplace-v1/pkg/repsonses"
	"github.com/labstack/echo/v4"
)

func (d *Delivery) AddFavoritesRoutes() {
	g := d.router.Group("favorites")

	g.POST("", d.AddToFavorites, authorization.Middleware)
	g.GET("", d.GetProductList, authorization.Middleware)
}

func (d *Delivery) AddToFavorites(c echo.Context) error {
	const op = "delivery.favorites.AddToFavorites"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	var vm *favorites.FavoriteProductVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidRequestBody))
	}

	dto := favorites.MapFavoriteProductVmToDto(claims.AccountId, vm)

	err = d.ucFavorites.AddProduct(ctx, dto)
	if err != nil {
		d.logError(op, ErrAddingProductToFavorites, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrAddingProductToFavorites))
	}

	return c.NoContent(http.StatusOK)
}

func (d *Delivery) GetProductList(c echo.Context) error {
	const op = "delivery.favorites.GetProductList"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	offsetParam := c.QueryParam("offset")
	offset, err := strconv.Atoi(offsetParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidQueryParam))
	}

	limitParam := c.QueryParam("limit")
	limit, err := strconv.Atoi(limitParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidQueryParam))
	}

	pagination := &usecase.PaginationDto{
		Offset: offset,
		Limit:  limit,
	}

	products, err := d.ucFavorites.GetProductList(ctx, claims.AccountId, pagination)
	if err != nil {
		d.logError(op, ErrGettingProductList, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrGettingProductList))
	}

	vm := &favorites.FavoriteList{
		ProductIds: make([]int, 0, len(products)),
	}

	for _, product := range products {
		vm.ProductIds = append(vm.ProductIds, product.ProductId)
	}

	return c.JSON(http.StatusOK, vm)
}

func (d *Delivery) DeleteProduct(c echo.Context) error {
	const op = "delivery.favorites.DeleteProduct"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	var vm *favorites.FavoriteProductVm
	err := c.Bind(&vm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidRequestBody))
	}

	dto := favorites.MapFavoriteProductVmToDto(claims.AccountId, vm)

	err = d.ucFavorites.DeleteProduct(ctx, dto)
	if err != nil {
		d.logError(op, ErrDeletingProductFromFavorites, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrDeletingProductFromFavorites))
	}

	return c.NoContent(http.StatusOK)
}
