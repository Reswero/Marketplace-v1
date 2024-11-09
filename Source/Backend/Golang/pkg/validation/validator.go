package validation

import "github.com/go-playground/validator/v10"

// Валидатор
type Validator struct {
	validator *validator.Validate
}

// Создать новый валидатор
func NewValidator() *Validator {
	return &Validator{
		validator: validator.New(),
	}
}

// Проверить структуру на валидность
func (v *Validator) Validate(i interface{}) error {
	if err := v.validator.Struct(i); err != nil {
		return err
	}

	return nil
}
