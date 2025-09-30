using Vkr.Domain.DTO.Journal;

namespace Vkr.Application.Interfaces.Repositories.JournalRepositories;

public interface IAuditLogRepository
{
    Task<IEnumerable<ChangeLogDto>> GetLogsByEntityAsync<T>(int entityId);
}