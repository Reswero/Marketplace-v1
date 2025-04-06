namespace Marketplace.Orders.Application.Common.Models.Services.Payment;

/// <summary>
/// Информация о платеже
/// </summary>
public class PaymentInfo(long orderId, int totalPrice)
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long OrderId { get; private set; } = orderId;
    /// <summary>
    /// Итоговая цена
    /// </summary>
    public int TotalPrice { get; private set; } = totalPrice;
}
