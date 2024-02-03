package http

const (
	ErrInvalidRequestBody   = "Неверное тело запроса"
	ErrAccountNotCreated    = "Ошибка при создании аккаунта"
	ErrAccountNotRetrieved  = "Ошибка при получении аккаунта"
	ErrSessionNotCreated    = "Ошибка при создании сессии"
	ErrWrongPhoneOrPassword = "Неверный номер телефона или пароль"
	ErrSessionExpired       = "Срок действия сессии истек"
	ErrSessionNotRetrieved  = "Ошибка при получении сессии"
)
