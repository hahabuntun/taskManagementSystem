using Vkr.Domain.DTO.Journal;

namespace Vkr.Application.Interfaces.Services.JournalServices;

public interface IAuditService
{
    /// <summary>
    /// Получить логи по конкретному типу T и его идентификатору
    /// </summary>
    Task<IEnumerable<ChangeLogDto>> GetLogsAsync<T>(int entityId);

    /// <summary>
    /// Универсальный метод: по строковому имени сущности и её id
    /// </summary>
    Task<IEnumerable<ChangeLogDto>> GetLogsAsync(string entityName, int entityId);
    
    /// <summary>
    /// Собирает читаемую историю изменений для сущности T.
    /// </summary>
    Task<string> GetHistorySummaryAsync<T>(int entityId);
}