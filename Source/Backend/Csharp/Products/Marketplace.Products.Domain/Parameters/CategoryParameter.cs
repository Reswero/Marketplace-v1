namespace Marketplace.Products.Domain.Parameters;

/// <summary>
/// Параметр категории
/// </summary>
public class CategoryParameter
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Тип
    /// </summary>
    public ParameterType Type { get; set; }
}
