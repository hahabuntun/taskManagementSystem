using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vkr.Domain.Entities.Journal;

namespace Vkr.DataAccess.Journal;

/// <summary>
/// Базовый DbContext с поддержкой аудита изменений.
/// Наследник должен реализовать способ получения текущего пользователя.
/// </summary>
public abstract class AuditableDbContext(DbContextOptions options) : DbContext(options)
{
     /// <summary>
    /// Таблица аудита — хранит все изменения сущностей.
    /// </summary>
    public DbSet<ChangeLog> ChangeLogs { get; set; } = null!;

    public override int SaveChanges()
    {
        var auditEntries = PrepareAuditEntries();
        var result = base.SaveChanges();
        if (auditEntries.Count <= 0) return result;
        FinalizeAuditEntries(auditEntries);
        base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        var auditEntries = PrepareAuditEntries();
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        if (auditEntries.Count <= 0) return result;
        
        FinalizeAuditEntries(auditEntries);
        await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        return result;
    }

    /// <summary>
    /// Собираем список AuditEntry (ChangeLog + EntityEntry), но не добавляем в БД.
    /// </summary>
    private List<AuditEntry> PrepareAuditEntries()
    {
        var entries = ChangeTracker.Entries()
            .Where(e =>
                e.Entity is not ChangeLog &&
                (e.State == EntityState.Added ||
                 e.State == EntityState.Modified ||
                 e.State == EntityState.Deleted))
            .ToList();

        var auditEntries = new List<AuditEntry>();

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;
            var changeType = entry.State.ToString();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey() || prop.IsTemporary)
                    continue;

                string? oldValue = null, newValue = null;
                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                    oldValue = prop.OriginalValue?.ToString();
                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                    newValue = prop.CurrentValue?.ToString();
                if (entry.State == EntityState.Modified && oldValue == newValue)
                    continue;

                var log = new ChangeLog
                {
                    EntityName   = entityName,
                    // EntityKey заполним только после SaveChanges()
                    EntityKey    = null,
                    PropertyName = prop.Metadata.Name,
                    OldValue     = oldValue,
                    NewValue     = newValue,
                    ChangeType   = changeType,
                    ChangedOn    = DateTime.UtcNow,
                    ChangedBy    = GetCurrentUserId()
                };

                auditEntries.Add(new AuditEntry { Log = log, Entry = entry });
            }
        }

        return auditEntries;
    }

    /// <summary>
    /// После того как реальные ключи проставлены, заполняем EntityKey и сохраняем логи.
    /// </summary>
    private void FinalizeAuditEntries(List<AuditEntry> auditEntries)
    {
        foreach (var audit in auditEntries)
        {
            // Получаем текущее значение первичного ключа
            var pkProp = audit.Entry.Properties
                .First(p => p.Metadata.IsPrimaryKey());
            var realKey = pkProp.CurrentValue?.ToString();

            audit.Log.EntityKey = realKey;
            ChangeLogs.Add(audit.Log);
        }
    }
    
    private class AuditEntry
    {
        public ChangeLog Log { get; set; } = null!;
        public EntityEntry Entry { get; set; } = null!;
    }

    protected abstract string? GetCurrentUserId();
}