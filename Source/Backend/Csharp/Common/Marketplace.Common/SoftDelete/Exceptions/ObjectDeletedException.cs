namespace Marketplace.Common.SoftDelete.Exceptions;

/// <summary>
/// Объект уже удален
/// </summary>
public class ObjectDeletedException() : Exception($"Объект уже удален") { }
