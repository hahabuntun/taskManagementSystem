using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.DTO.Task;

namespace Vkr.Application.Interfaces.Services.TaskServices;

public interface ITaskMessageService
{
    Task<IEnumerable<TaskMessageDTO>> GetMessagesByTaskAsync(int taskId);
    Task<int> GetMessageCountByTaskAsync(int taskId);
    Task<TaskMessageDTO> CreateMessageAsync(TaskMessageCreateDTO dto); // SenderId already in DTO
    Task UpdateMessageAsync(int messageId, string newText, int creatorId);
    Task DeleteMessageAsync(int messageId, int creatorId);
}