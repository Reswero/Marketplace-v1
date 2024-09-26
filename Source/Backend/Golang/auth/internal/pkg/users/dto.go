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
	Id        int    `json:"id"`
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}

type CreateCustomerDto struct {
	Id        int    `json:"id"`
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}

type CreateSellerDto struct {
	Id          int    `json:"id"`
	FirstName   string `json:"firstName"`
	LastName    string `json:"lastName"`
	CompanyName string `json:"companyName"`
}

type CreateStaffDto struct {
	Id        int    `json:"id"`
	FirstName string `json:"firstName"`
	LastName  string `json:"lastName"`
}
