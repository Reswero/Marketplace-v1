namespace Marketplace.Orders.Application.Common.Models;

/// <summary>
/// Пагинация
/// </summary>
public class Pagination
{
    private const int _maxLimit = 200;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Ограничение</param>
    public Pagination(int offset, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        Offset = offset;
        Limit = limit < _maxLimit ? limit : _maxLimit;
    }

    /// <summary>
    /// Смещение
    /// </summary>
    public int Offset { get; set; }
    /// <summary>
    /// Ограничение
    /// </summary>
    public int Limit { get; set; }
}
