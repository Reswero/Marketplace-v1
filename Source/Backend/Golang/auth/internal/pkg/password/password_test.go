package password_test

import (
	"strings"
	"testing"

	"github.com/Reswero/Marketplace-v1/auth/internal/pkg/password"
)

func TestPasswordHashing(t *testing.T) {
	pass := "testpass"

	hashed, salt, err := password.Hash(pass)
	if err != nil {
		t.Errorf("Error: %v", err)
	}
	if !strings.HasPrefix(hashed, "$2a$10$") {
		t.Error("Error: Password should be hashed with bcrypt")
	}
	if len(salt) != password.SaltLength {
		t.Errorf("Error: Expected salt length %d, got %d", len(salt), password.SaltLength)
	}

	t.Logf("Hashed pass: %s, Salt: %s", hashed, salt)
}

func TestPasswordValidating(t *testing.T) {
	pass := "testpass"
	hashed, salt, err := password.Hash(pass)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	valid := password.Validate(hashed, pass, salt)

	if !valid {
		t.Errorf("Error: Password should be valid")
	}
}
