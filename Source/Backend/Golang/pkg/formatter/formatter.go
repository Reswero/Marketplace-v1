package formatter

import "fmt"

// Добавляет название операции к тексту ошибки
func FmtError(op string, err error) error {
	return fmt.Errorf("%s: %w", op, err)
}
