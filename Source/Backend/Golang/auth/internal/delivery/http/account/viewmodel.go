package account

type PersonVm struct {
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}

type AccountCreateVm struct {
	PhoneNumber string `json:"phoneNumber"`
	Email       string `json:"email"`
	Password    string `json:"password"`
}

type CustomerAccountCreateVm struct {
	AccountCreateVm
	PersonVm
}

type SellerAccountCreateVm struct {
	AccountCreateVm
	PersonVm
	CompanyName string `json:"companyName"`
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
	Id          int    `json:"id"`
	PhoneNumber string `json:"phoneNumber"`
	Email       string `json:"email"`
}

type AccountCreatedVm struct {
	Id int `json:"id"`
}

type ChangePasswordVm struct {
	OldPassword string `json:"oldPassword"`
	NewPassword string `json:"newPassword"`
}

type ChangeEmailVm struct {
	NewEmail string `json:"newEmail"`
	Password string `json:"password"`
}
