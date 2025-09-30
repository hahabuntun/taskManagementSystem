using System.Text;
using AutoMapper;
using Vkr.Application.Interfaces.Repositories.JournalRepositories;
using Vkr.Application.Interfaces.Services.JournalServices;
using Vkr.Domain.DTO.Journal;

namespace Vkr.Application.Services.JournalServices;

public class AuditService(IAuditLogRepository repo, IMapper mapper) : IAuditService
{
    // Словарь: свойство → дружелюбное название
    private static readonly Dictionary<string,string> DisplayNames = new()
    {
        { "Title",     "Название"        },
        { "Description","Описание"       },
        { "StartsOn",  "Дата начала"     },
        { "ExpireOn",  "Дата окончания"  },
        { "CreatedOn", "Дата создания"   },
        // добавить другие по необходимости
    };

    // Список свойств, которые не следует логировать вовсе
    private static readonly HashSet<string> SensitiveProperties = new(StringComparer.OrdinalIgnoreCase)
    {
        "PasswordHash",
        "SecurityStamp",
        "Email",            // если email считается чувствительным
        // и т.д.
    };
    
    public Task<IEnumerable<ChangeLogDto>> GetLogsAsync<T>(int entityId)
        => repo.GetLogsByEntityAsync<T>(entityId);

    public async Task<IEnumerable<ChangeLogDto>> GetLogsAsync(string entityName, int entityId)
    {
        // 1. Находим CLR-тип сущности по её имени
        var entityType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => 
                t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

        if (entityType == null)
            throw new ArgumentException($"Unknown entity type: {entityName}", nameof(entityName));

        // 2. Берём methodInfo для GetLogsByEntityAsync<T>
        var method = typeof(IAuditLogRepository)
            .GetMethod(nameof(IAuditLogRepository.GetLogsByEntityAsync))
            ?.MakeGenericMethod(entityType);

        if (method == null)
            throw new InvalidOperationException(
                "Метод GetLogsByEntityAsync<T> не найден в IAuditLogRepository");

        // 3. Вызываем через reflection
        var task = (Task<IEnumerable<ChangeLogDto>>)method.Invoke(repo, new object[] { entityId })!;
        return await task;
    }
    
    public async Task<string> GetHistorySummaryAsync<T>(int entityId)
    {
        var logs = (await repo.GetLogsByEntityAsync<T>(entityId))
            .OrderBy(l => l.ChangedOn)
            .Where(l => !SensitiveProperties.Contains(l.PropertyName))
            .ToList();

        if (!logs.Any())
            return "Для данной записи нет изменений.";

        var lines = new List<string>(logs.Count);
        foreach (var log in logs)
        {
            var when = log.ChangedOn
                .ToLocalTime()
                .ToString("dd.MM.yyyy HH:mm");

            var field = DisplayNames.TryGetValue(log.PropertyName, out var name)
                ? name
                : log.PropertyName;

            var action = log.ChangeType switch
            {
                "Added"   => $"добавлено «{log.NewValue}»",
                "Deleted" => $"удалено (бывшее «{log.OldValue}»)",
                _         => $"изменено с «{log.OldValue}» на «{log.NewValue}»"
            };

            lines.Add($"• {when}: {field} {action}. Пользователь: {log.ChangedBy}");
        }

        return string.Join(Environment.NewLine, lines);
    }
}