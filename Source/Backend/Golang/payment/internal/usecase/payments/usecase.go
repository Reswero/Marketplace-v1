package payments

import "context"

type UseCase struct {
}

func (u *UseCase) GetLink(ctx context.Context, orderId int64, paybleAmount int) (string, error) {
	const op = "usecase.payments.GetLink"

	return "sample-url", nil
}
