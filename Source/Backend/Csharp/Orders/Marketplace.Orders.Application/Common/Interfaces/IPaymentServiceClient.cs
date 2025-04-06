using Marketplace.Orders.Application.Common.Models.Services.Payment;

namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Клиент к сервису платежей
/// </summary>
public interface IPaymentServiceClient
{
    /// <summary>
    /// Создать платёж
    /// </summary>
    /// <param name="info">Информация о платеже</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификатор платежа</returns>
    public Task<string> CreatePaymentAsync(PaymentInfo info, CancellationToken cancellationToken = default);
}
