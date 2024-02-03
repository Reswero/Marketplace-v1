package http

const (
	ErrInvalidRequestBody   = "Неверное тело запроса"
	ErrInvalidParam         = "Неверный параметр"
	ErrAccountNotCreated    = "Ошибка при создании аккаунта"
	ErrAccountNotRetrieved  = "Ошибка при получении аккаунта"
	ErrAccountNotFound      = "Аккаунт не найден"
	ErrSessionNotCreated    = "Ошибка при создании сессии"
	ErrWrongPhoneOrPassword = "Неверный номер телефона или пароль"
	ErrWrongPassword        = "Неверный пароль"
	ErrSessionExpired       = "Срок действия сессии истек"
	ErrSessionNotRetrieved  = "Ошибка при получении сессии"
	ErrPasswordNotChanged   = "Ошибка при смене пароля"
)
