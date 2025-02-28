using Marketplace.Orders.Domain;

namespace Marketplace.Orders.Application.Common.Interfaces;

/// <summary>
/// Репозиторий новых заказов
/// </summary>
public interface INewOrdersRepository
{
    /// <summary>
    /// Добавить новый заказ
    /// </summary>
    /// <param name="newOrder">Новый заказ</param>
    /// <param name="cancellationToken"></param>
    public Task AddAsync(NewOrder newOrder, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить новые заказы
    /// </summary>
    /// <param name="limit">Количество</param>
    /// <param name="cancellationToken"></param>
    public Task<List<NewOrder>> GetAsync(int limit, CancellationToken cancellationToken = default);
    /// <summary>
    /// Удалить новый заказ
    /// </summary>
    /// <param name="order">Новый заказ</param>
    /// <param name="cancellationToken"></param>
    public Task DeleteAsync(NewOrder order, CancellationToken cancellationToken = default);
}
