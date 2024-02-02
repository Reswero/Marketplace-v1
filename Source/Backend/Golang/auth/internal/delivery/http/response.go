package http

type StatusResponse struct {
	Status  int    `json:"status"`
	Message string `json:"message"`
}

func NewStatusResp(status int, message string) *StatusResponse {
	return &StatusResponse{
		Status:  status,
		Message: message,
	}
}
