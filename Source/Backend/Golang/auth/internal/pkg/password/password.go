package password

import (
	"github.com/Reswero/Marketplace-v1/pkg/generator"
	"golang.org/x/crypto/bcrypt"
)

const SaltLength = 8

func Hash(password string) (string, string, error) {
	salt, err := generator.GetRandomAsciiString(SaltLength)
	if err != nil {
		return "", "", err
	}
	saltedPass := password + salt

	hashedPass, err := bcrypt.GenerateFromPassword([]byte(saltedPass), bcrypt.DefaultCost)
	if err != nil {
		return "", "", err
	}

	return string(hashedPass), salt, nil
}

func Validate(hashedPassword, password, salt string) bool {
	err := bcrypt.CompareHashAndPassword([]byte(hashedPassword), []byte(password+salt))
	return err == nil
}
