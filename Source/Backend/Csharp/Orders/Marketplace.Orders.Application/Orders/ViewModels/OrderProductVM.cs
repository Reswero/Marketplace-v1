namespace Marketplace.Orders.Application.Orders.ViewModels;

/// <summary>
/// Модель товара заказа
/// </summary>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="Quantity">Количество</param>
/// <param name="ProductPrice">Цена товара</param>
/// <param name="DiscountSize">Размер скидки</param>
public record OrderProductVM(int ProductId, int Quantity, int ProductPrice, int DiscountSize);
