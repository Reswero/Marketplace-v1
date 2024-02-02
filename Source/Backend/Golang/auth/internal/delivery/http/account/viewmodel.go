package account

type PersonVm struct {
	FirstName string
	LastName  string
}

type AccountCreateVm struct {
	PhoneNumber string
	Email       string
	Password    string
}

type CustomerAccountCreateVm struct {
	AccountCreateVm
	PersonVm
}

type SellerAccountCreateVm struct {
	AccountCreateVm
	PersonVm
	CompanyName string
}

type StaffAccountCreateVm struct {
	AccountCreateVm
	PersonVm
}

type AdminAccountCreateVm struct {
	AccountCreateVm
	PersonVm
}

type AccountVm struct {
	Id          int
	PhoneNumber string
	Email       string
	Password    string
}

type AccountCreatedVm struct {
	Id int `json:"id"`
}
