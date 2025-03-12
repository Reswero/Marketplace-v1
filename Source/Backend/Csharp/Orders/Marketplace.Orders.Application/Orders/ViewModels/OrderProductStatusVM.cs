using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель статуса товара
/// </summary>
/// <param name="Type">Тип статуса</param>
/// <param name="OccuredAt">Произошло в</param>
public record OrderProductStatusVM(OrderProductStatusType Type, DateTimeOffset OccuredAt);
