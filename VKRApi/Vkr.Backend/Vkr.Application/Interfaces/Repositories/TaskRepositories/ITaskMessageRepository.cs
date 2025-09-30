using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.TaskRepositories;

public interface ITaskMessageRepository
{
    Task<IEnumerable<TaskMessage>> GetByTaskIdAsync(int taskId);
    Task<int> GetMessageCountForTaskAsync(int taskId);

    // Создание и обновление
    Task<int> CreateAsync(TaskMessageCreateDTO dto);
    Task UpdateMessageTextAsync(int messageId, string newText);

    // Удаление
    Task DeleteAsync(int id);
    Task<TaskMessage?> GetByIdAsync(int id);
}