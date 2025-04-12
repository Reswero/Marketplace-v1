namespace Marketplace.Delivery.Application.Common.Exceptions;

public class DeliveryNotFoundException(long orderId)
    : Exception($"Не найдена доставка для заказа с идентификатором {orderId}")
{ }
