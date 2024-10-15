namespace Marketplace.Products.Application.Common.Exceptions;

/// <summary>
/// Объект не найден
/// </summary>
public class ObjectNotFoundException : Exception
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="type">Тип</param>
    public ObjectNotFoundException(Type type)
        : base($"Объект типа \"{type.Name}\" не найден") { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="type">Тип</param>
    /// <param name="id">Идентификатор</param>
    public ObjectNotFoundException(Type type, int id)
        : base($"Объект типа \"{type.Name}\" с идентификатором {id} не найден") { }
}
