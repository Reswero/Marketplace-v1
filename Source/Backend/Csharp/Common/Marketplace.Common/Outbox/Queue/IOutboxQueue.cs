using Marketplace.Common.Outbox.Exceptions;

namespace Marketplace.Common.Outbox.Queue;

/// <summary>
/// Паттерн Outbox на основе очереди
/// </summary>
/// <typeparam name="T">Тип элемента</typeparam>
public interface IOutboxQueue<T>
{
    /// <summary>
    /// Добавить элемент в конец очереди
    /// </summary>
    /// <param name="item">Элемент</param>
    /// <param name="cancellationToken"></param>
    public Task PushAsync(T item, CancellationToken cancellationToken = default);
    /// <summary>
    /// Добавить элементы в конец очереди
    /// </summary>
    /// <param name="items">Элементы</param>
    /// <param name="cancellationToken"></param>
    public Task PushAsync(T[] items, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить первый элемент из очереди
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <exception cref="OutboxEmptyException" />
    public Task<T> PeekAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить несколько первых элементов из очереди
    /// </summary>
    /// <param name="count">Количество</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="OutboxEmptyException" />
    public Task<List<T>> PeekAsync(int count, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить и удалить первый элемент из очереди
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <exception cref="OutboxEmptyException" />
    public Task<T> PopAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить и удалить несколько первыз элментов из очереди
    /// </summary>
    /// <param name="count">Количество</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="OutboxEmptyException" />
    public Task<List<T>> PopAsync(int count, CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить количество элементов в очереди
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<int> CountAsync(CancellationToken cancellationToken = default);
}
