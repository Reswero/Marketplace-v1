using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель статуса заказа
/// </summary>
/// <param name="Type">Тип статуса</param>
/// <param name="OccuredAt">Произошло в</param>
public record OrderStatusVM(OrderStatusType Type, DateTimeOffset OccuredAt);
