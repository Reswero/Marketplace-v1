package generator

import "crypto/rand"

const symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

// Генерирует случайную строчку указанной длины в ASCII-формате
func GetRandomAsciiString(length int) (string, error) {
	buffer := make([]byte, length)

	_, err := rand.Read(buffer)
	if err != nil {
		return "", err
	}

	len := byte(len(symbols))
	for i, v := range buffer {
		buffer[i] = symbols[v%len]
	}

	return string(buffer), nil
}
