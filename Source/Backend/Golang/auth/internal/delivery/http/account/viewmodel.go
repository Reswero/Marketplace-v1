package account

type PersonVm struct {
	FirstName string `json:"firstName" validate:"required,min=2,max=20"`
	LastName  string `json:"lastName" validate:"required,min=2,max=40"`
}

type AccountCreateVm struct {
	PhoneNumber string `json:"phoneNumber" validate:"required,e164"`
	Email       string `json:"email" validate:"required,email"`
	Password    string `json:"password" validate:"required,min=6,max=100"`
}

type CustomerAccountCreateVm struct {
	AccountCreateVm
	PersonVm
}

type SellerAccountCreateVm struct {
	AccountCreateVm
	PersonVm
	CompanyName string `json:"companyName" validate:"required,min=6,max=100"`
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
	OldPassword string `json:"oldPassword" validate:"required"`
	NewPassword string `json:"newPassword" validate:"required"`
}

type ChangeEmailVm struct {
	NewEmail string `json:"newEmail" validate:"required,email"`
	Password string `json:"password" validate:"required"`
}
