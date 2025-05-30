﻿namespace Marketplace.Orders.Domain;

/// <summary>
/// Статус товара из заказа
/// </summary>
public class OrderProductStatus
{
    private OrderProductStatus() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="product">Товара из заказа</param>
    /// <param name="type">Тип</param>
    public OrderProductStatus(OrderProduct product, OrderProductStatusType type)
    {
        OrderProductId = product.ProductId;
        OrderProduct = product;
        Type = type;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="product">Товара из заказа</param>
    /// <param name="type">Тип</param>
    /// <param name="occuredAt">Произошло в</param>
    public OrderProductStatus(OrderProduct product, OrderProductStatusType type, DateTimeOffset occuredAt)
    {
        OrderProductId = product.ProductId;
        OrderProduct = product;
        Type = type;
        OccuredAt = occuredAt;
    }

    /// <summary>
    /// Идентификатор товара из заказа
    /// </summary>
    public long OrderProductId { get; private set; }
    /// <summary>
    /// Товар из заказа
    /// </summary>
    public OrderProduct? OrderProduct { get; private set; }
    /// <summary>
    /// Статус
    /// </summary>
    public OrderProductStatusType Type { get; private set; }
    /// <summary>
    /// Произошло в
    /// </summary>
    public DateTimeOffset OccuredAt { get; private set; } = DateTimeOffset.UtcNow;
}
