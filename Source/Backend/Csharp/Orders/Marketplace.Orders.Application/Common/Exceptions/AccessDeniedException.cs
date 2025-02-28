namespace Marketplace.Orders.Application.Common.Exceptions;

/// <summary>
/// Доступ запрещён
/// </summary>
public class AccessDeniedException()
    : Exception("Доступ запрещён")
{ }
