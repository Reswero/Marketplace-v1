package favorites

type FavoriteProductVm struct {
	ProductId int `json:"productId"`
}

type FavoriteListVm struct {
	ProductIds []int `json:"productIds"`
}

type CheckFavoritesProductsVm struct {
	ProductIdsToCheck []int `json:"productIdsToCheck"`
}
