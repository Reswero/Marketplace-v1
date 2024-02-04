package session

type SessionVm struct {
	Id string `json:"id"`
}

type CredentialsVm struct {
	PhoneNumber string `json:"phoneNumber" validate:"required,e164"`
	AccountType string `json:"accountType" validate:"required"`
	Password    string `json:"password" validate:"required"`
}
