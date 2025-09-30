using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.JournalRepositories;
using Vkr.Domain.DTO.Journal;

namespace Vkr.DataAccess.Repositories.JournalRepositories;

public class AuditLogRepository(ApplicationDbContext context) : IAuditLogRepository
{
    /// <summary>
    /// Получить все записи аудита для сущности с заданным именем и ключом,
    /// проецируя сразу в DTO.
    /// </summary>
    private async Task<IEnumerable<ChangeLogDto>> GetLogsByEntityAsync(string entityName, string entityKey)
    {
        return await context.ChangeLogs
            .AsNoTracking()
            .Where(cl => cl.EntityName == entityName && cl.EntityKey == entityKey)
            .OrderBy(cl => cl.ChangedOn)
            .Select(cl => new ChangeLogDto
            {
                Id           = cl.Id,
                EntityName   = cl.EntityName,
                EntityKey    = cl.EntityKey,
                PropertyName = cl.PropertyName,
                OldValue     = cl.OldValue,
                NewValue     = cl.NewValue,
                ChangeType   = cl.ChangeType,
                ChangedOn    = cl.ChangedOn,
                ChangedBy    = cl.ChangedBy
            })
            .ToListAsync();
    }

    /// <summary>
    /// Удобный overload: по типу сущности T и её целочисленному ключу.
    /// </summary>
    public Task<IEnumerable<ChangeLogDto>> GetLogsByEntityAsync<T>(int entityId)
    {
        return GetLogsByEntityAsync(typeof(T).Name, entityId.ToString());
    }
}