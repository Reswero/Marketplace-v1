package generator_test

import (
	"testing"

	"github.com/Reswero/Marketplace-v1/pkg/generator"
)

func TestRandomAsciiStringGenerator(t *testing.T) {
	length := 20

	s, err := generator.GetRandomAsciiString(length)
	if err != nil {
		t.Errorf("Error: %v", err)
	}

	if len(s) != length {
		t.Errorf("Error: Expected length %d, got %d", length, len(s))
	}
	t.Logf("Generated string: %s", s)
}
