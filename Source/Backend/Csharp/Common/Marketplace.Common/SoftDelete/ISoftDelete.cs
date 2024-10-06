﻿namespace Marketplace.Common.SoftDelete;

/// <summary>
/// Паттерн Soft Delete
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Время удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; }
    /// <summary>
    /// Пометить объект как удаленный
    /// </summary>
    public void SetDeleted();
}