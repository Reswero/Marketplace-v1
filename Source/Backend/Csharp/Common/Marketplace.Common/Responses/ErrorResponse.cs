namespace Marketplace.Common.Responses;

/// <summary>
/// Ответ с ошибкой
/// </summary>
/// <param name="Status">Статус</param>
/// <param name="Message">Сообщение</param>
public record ErrorResponse(int Status, string Message);
