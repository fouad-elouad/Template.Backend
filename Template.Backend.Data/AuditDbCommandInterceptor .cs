using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Template.Backend.Data.Utilities;
using Template.Backend.Model;
using Template.Backend.Model.Audit;
using Template.Backend.Model.Enums;

namespace Template.Backend.Data
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public AuditSaveChangesInterceptor(ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            AuditUpdatedEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            AuditUpdatedEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void AuditUpdatedEntities(DbContext? context)
        {
            if (context == null) return;

            var CreatedDate = _dateTime.Now;
            foreach (var entry in context.ChangeTracker.Entries<IEntity>().Where(e => e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted || e.HasChangedOwnedEntities()).ToList())
            {
                var entityType = entry.Entity.GetType();
                if (StarterDbContext._auditTypesMapping.ContainsKey(entityType))
                {
                    Type auditType = StarterDbContext._auditTypesMapping[entityType];
                    var auditProperties = auditType.GetProperties();
                    EntityEntry<IAuditEntity> auditEntityEntry = context.GetEntityEntry<IAuditEntity>(auditType);

                    // EntityState.Modified
                    if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                    {
                        var entityProperties = entry.CurrentValues.Properties.Select(p => p.Name).ToList();

                        auditEntityEntry.Entity.CreatedDate = CreatedDate;
                        auditEntityEntry.Entity.AuditOperation = AuditOperations.UPDATE;
                        // GetDatabaseValues() of RowVersion prevent user to set a wrong value
                        entry.Entity.RowVersion = entry.GetDatabaseValues().GetValue<int>(nameof(entry.Entity.RowVersion)) + 1;
                        entry.Entity.CreatedOn = entry.GetDatabaseValues().GetValue<DateTime?>(nameof(entry.Entity.CreatedOn));
                        auditEntityEntry.Entity.LoggedUserName = _currentUserService.UserId;

                        foreach (var property in auditProperties)
                        {
                            if (entityProperties.Contains(property.Name))
                            {
                                auditEntityEntry.Property(property.Name).CurrentValue = entry.Property(property.Name).CurrentValue;
                            }
                        }
                    }

                    // EntityState.Deleted
                    else if (entry.State == EntityState.Deleted)
                    {
                        var entityProperties = entry.OriginalValues.Properties.Select(p => p.Name).ToList();

                        foreach (var property in auditProperties)
                        {
                            if (entityProperties.Contains(property.Name))
                            {
                                auditEntityEntry.Property(property.Name).CurrentValue = entry.Property(property.Name).OriginalValue;
                            }
                        }

                        auditEntityEntry.Entity.AuditOperation = AuditOperations.DELETE;
                        auditEntityEntry.Entity.CreatedDate = CreatedDate;
                        auditEntityEntry.Entity.LoggedUserName = _currentUserService.UserId;
                    }
                }
            }
        }
    }

    public static class EntityEntryExtensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}