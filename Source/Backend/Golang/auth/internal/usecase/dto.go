package usecase

type PersonDto struct {
	FirstName string
	LastName  string
}

type AccountDto struct {
	Id          int
	PhoneNumber string
	Email       string
	Password    string
}

type CustomerAccountDto struct {
	AccountDto
	PersonDto
}

type SellerAccountDto struct {
	AccountDto
	PersonDto
	CompanyName string
}

type StaffAccountDto struct {
	AccountDto
	PersonDto
}

type AdminAccountDto struct {
	AccountDto
	PersonDto
}
