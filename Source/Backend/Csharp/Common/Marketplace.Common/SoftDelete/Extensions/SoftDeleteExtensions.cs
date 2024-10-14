using Marketplace.Common.SoftDelete.Exceptions;

namespace Marketplace.Common.SoftDelete.Extensions;

/// <summary>
/// Расширения для интерфейса <see cref="ISoftDelete"/>
/// </summary>
public static class SoftDeleteExtensions
{
    /// <summary>
    /// Выбросить исключение, если объект удален
    /// </summary>
    /// <param name="softDelete"></param>
    /// <exception cref="ObjectDeletedException"></exception>
    public static void ThrowIfDeleted(this ISoftDelete softDelete)
    {
        if (softDelete.DeletedAt is null)
            return;

        throw new ObjectDeletedException();
    }
}
