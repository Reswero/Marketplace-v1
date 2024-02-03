package session

type SessionVm struct {
	Id string `json:"id"`
}

type CredentialsVm struct {
	PhoneNumber string `json:"phoneNumber"`
	AccountType string `json:"accountType"`
	Password    string `json:"password"`
}
