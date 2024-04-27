using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartMaintApi.Models;

namespace SmartMaintApi.Interceptors;

public sealed class UpdateEntityInfoInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
         DbContextEventData eventData,
         InterceptionResult<int> result,
         CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;
        var entities = context.ChangeTracker.Entries().ToList();

        foreach (EntityEntry entry in entities)
        {
            if (entry.State == EntityState.Added)
                UpdateAddedEntity(entry, utcNow);

            else if (entry.State == EntityState.Modified)
                UpdateModifiedEntity(entry, utcNow);
        }
    }

    private static void UpdateAddedEntity(EntityEntry entry, DateTime utcNow)
    {
        UpdatePropertyIfExists(entry, nameof(EntityInfo.Created), utcNow);

        // TO DO: Implement user who made the query/logged in
        UpdatePropertyIfExists(entry, nameof(EntityInfo.CreatedBy), "EntityAdded_User_Test_CreateUser");
    }

    private static void UpdateModifiedEntity(EntityEntry entry, DateTime utcNow)
    {
        UpdatePropertyIfExists(entry, nameof(EntityInfo.Modified), utcNow);

        // TO DO: Implement user who made the query/logged in
        UpdatePropertyIfExists(entry, nameof(EntityInfo.ModifiedBy), "EntityAdded_User_Test_ModifiedUser");
    }

    private static void UpdatePropertyIfExists(EntityEntry entry, string propertyName, object value)
    {
        if (entry.Metadata.FindProperty(propertyName) != null)
            entry.Property(propertyName).CurrentValue = value;
    }
}
