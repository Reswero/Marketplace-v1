package viewmodel

type Product struct {
	Id    int `json:"id"`
	Count int `json:"count"`
}

type AddProduct struct {
	Id int `json:"id"`
}

type AddCount struct {
	Count int `json:"count"`
}
