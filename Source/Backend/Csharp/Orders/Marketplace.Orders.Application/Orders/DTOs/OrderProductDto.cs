namespace Marketplace.Orders.Application.Orders.DTOs;

/// <summary>
/// Товар заказа
/// </summary>
public record OrderProductDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; set; }
}
