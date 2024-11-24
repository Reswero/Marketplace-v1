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
	g := d.router.Group("/v1/favorites")

	g.POST("", d.AddToFavorites, authorization.Middleware)
	g.GET("", d.GetProductList, authorization.Middleware)
	g.DELETE("", d.DeleteProduct, authorization.Middleware)

	d.router.POST("/v1/internal/customers/:id/in-favorites", d.CheckProductsInFavorites)
}

// @id add-favorites
// @summary Добавление товара в избранное
// @description Добавление товара в избранное покупателя
// @router /favorites [post]
// @security AccountId
// @security AccountType
// @tags favorites
// @accept json
// @produce json
// @param favoriteProduct body favorites.FavoriteProductVm true "Избранный товар"
// @success 200
// @failure 400 {object} responses.StatusResponse
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
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

// @id get-favorites
// @summary Получение списка избранных товаров
// @description Получение списка избранных товаров покупателя
// @router /favorites [get]
// @security AccountId
// @security AccountType
// @tags favorites
// @accept json
// @produce json
// @param offset query int true "Смещение"
// @param limit query int true "Ограничение"
// @success 200 {object} favorites.FavoriteListVm
// @failure 400 {object} responses.StatusResponse
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
func (d *Delivery) GetProductList(c echo.Context) error {
	const op = "delivery.favorites.GetProductList"
	ctx := c.Request().Context()

	claims := authorization.GetClaims(c)

	offsetParam := c.QueryParam("offset")
	offset, err := strconv.Atoi(offsetParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidQueryParam))
	}

	if offset < 0 {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, ErrOffsetMustBeNotNegative))
	}

	limitParam := c.QueryParam("limit")
	limit, err := strconv.Atoi(limitParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidQueryParam))
	}

	if limit < 0 {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, ErrLimitMustBeNotNegative))
	} else if limit > 200 {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, ErrLimitExceededMaxValue))
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

	vm := &favorites.FavoriteListVm{
		ProductIds: make([]int, 0, len(products)),
	}

	for _, product := range products {
		vm.ProductIds = append(vm.ProductIds, product.ProductId)
	}

	return c.JSON(http.StatusOK, vm)
}

// @id delete-favorites
// @summary Удаление товара из избранного
// @description Удаление товара из списка избранного покупателя
// @router /favorites [delete]
// @security AccountId
// @security AccountType
// @tags favorites
// @accept json
// @produce json
// @param favoriteProduct body favorites.FavoriteProductVm true "Избранный товар"
// @success 200
// @failure 400 {object} responses.StatusResponse
// @failure 401 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
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

// @id check-favorites
// @summary Проверка находятся ли товары в избранном
// @description Проверка находятся ли товары в избранном у покупателя
// @router /internal/customers/{id}/in-favorites [post]
// @security AccountId
// @security AccountType
// @tags favorites
// @accept json
// @produce json
// @param id path int true "Идентификатор покупателя"
// @param list body favorites.CheckFavoritesProductsVm true "Список идентификаторов товаров для проверки"
// @success 200 {object} favorites.FavoriteListVm
// @failure 400 {object} responses.StatusResponse
// @failure 500 {object} responses.StatusResponse
func (d *Delivery) CheckProductsInFavorites(c echo.Context) error {
	const op = "delivery.favorites.CheckProductsInFavorites"
	ctx := c.Request().Context()

	customerIdParam := c.Param("id")
	customerId, err := strconv.Atoi(customerIdParam)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidParam))
	}

	var listVm *favorites.CheckFavoritesProductsVm
	err = c.Bind(&listVm)
	if err != nil {
		return c.JSON(http.StatusBadRequest, responses.NewStatus(http.StatusBadRequest, responses.ErrInvalidRequestBody))
	}

	ids, err := d.ucFavorites.CheckProductsInFavorite(ctx, customerId, listVm.ProductIdsToCheck)
	if err != nil {
		d.logError(op, ErrCheckingProductsInFavorites, err)
		return c.JSON(http.StatusInternalServerError, responses.NewStatus(http.StatusInternalServerError, ErrCheckingProductsInFavorites))
	}

	responseVm := &favorites.FavoriteListVm{
		ProductIds: ids,
	}
	return c.JSON(http.StatusOK, responseVm)
}
