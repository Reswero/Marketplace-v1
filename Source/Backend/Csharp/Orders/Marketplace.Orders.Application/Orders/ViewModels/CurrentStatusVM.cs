using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель текущего статуса
/// </summary>
/// <param name="Type">Тип</param>
/// <param name="OccuredAt">Произошло в</param>
public record CurrentStatusVM(OrderStatusType Type, DateTimeOffset OccuredAt);
