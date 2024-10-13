using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Marketplace.Common.SoftDelete;

/// <summary>
/// Перехватчик SavingChanges для Soft Delete
/// </summary>
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        MarkObjectsAsDeleted(eventData);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        MarkObjectsAsDeleted(eventData);
        return ValueTask.FromResult(result);
    }

    private void MarkObjectsAsDeleted(DbContextEventData eventData)
    {
        if (eventData.Context is null)
            return;

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete item)
            {
                entry.State = EntityState.Modified;
                item.SetDeleted();
            }
        }
    }
}
