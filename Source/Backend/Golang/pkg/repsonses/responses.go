package repsonses

type StatusResponse struct {
	Status  int    `json:"status"`
	Message string `json:"message"`
}

func NewStatus(status int, message string) *StatusResponse {
	return &StatusResponse{
		Status:  status,
		Message: message,
	}
}
