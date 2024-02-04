package http

import "github.com/go-playground/validator/v10"

type CustomValidator struct {
	validator *validator.Validate
}

func (v *CustomValidator) Validate(i interface{}) error {
	if err := v.validator.Struct(i); err != nil {
		return err
	}

	return nil
}
