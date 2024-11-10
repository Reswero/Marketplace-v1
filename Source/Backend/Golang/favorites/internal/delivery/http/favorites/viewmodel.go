package favorites

type FavoriteProductVm struct {
	ProductId int `json:"productId"`
}

type FavoriteList struct {
	ProductIds []int `json:"productIds"`
}
