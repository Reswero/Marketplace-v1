﻿using Marketplace.Orders.Application.Common.Models;
using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Репозиторий заказов
/// </summary>
public interface IOrdersRepository
{
    /// <summary>
    /// Добавить заказ
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(Order order, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить заказ
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    public Task<Order> GetAsync(long id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить заказы по покупателю
    /// </summary>
    /// <param name="customerId">Идентификатор покупателя</param>
    /// <param name="cancellationToken"></param>
    public Task<List<Order>> GetByCustomerAsync(int customerId, Pagination pagination, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновить заказ
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="cancellationToken"></param>
    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}
