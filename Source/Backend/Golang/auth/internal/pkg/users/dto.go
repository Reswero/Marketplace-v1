package users

type OutboxAction int

const (
	DeleteAction OutboxAction = iota
)

type OutboxDto struct {
	Action    OutboxAction `json:"action"`
	AccountId int          `json:"accountId"`
}

type CreateAdministratorDto struct {
	AccountId int    `json:"accountId"`
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}

type CreateCustomerDto struct {
	AccountId int    `json:"accountId"`
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}

type CreateSellerDto struct {
	AccountId   int    `json:"accountId"`
	FirstName   string `json:"firstName"`
	LastName    string `json:"lastName"`
	CompanyName string `json:"companyName"`
}

type CreateStaffDto struct {
	AccountId int    `json:"accountId"`
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}
