package account

import (
	"github.com/Reswero/Marketplace-v1/auth/internal/usecase/adapters/repository"
)

type UseCase struct {
	repo repository.Account
}

func New(r repository.Account) *UseCase {
	return &UseCase{
		repo: r,
	}
}

// func (u *UseCase) CreateCustomer(ctx context.Context, acc *usecase.CustomerAccountDto) (int, error) {

// }
