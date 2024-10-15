namespace Marketplace.Products.Application.Common.Exceptions;

/// <summary>
/// Объект уже существует
/// </summary>
/// <param name="type">Тип</param>
/// <param name="name">Название</param>
public class ObjectAlreadyExistsException(Type type, string name)
    : Exception($"Объект типа \"{type.Name}\" с названием \"{name}\" уже существует")
{ }
