using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.TaskRepositories;

public class TaskMessageRepository(ApplicationDbContext context) : ITaskMessageRepository
{
    public async Task<IEnumerable<TaskMessage>> GetByTaskIdAsync(int taskId)
    {
        return await context.TaskMessage
            .Where(tm => tm.RelatedTaskId == taskId)
            .Include(tm => tm.Sender)
            .OrderBy(tm => tm.CreatedOn)
            .ToListAsync();
    }

    public async Task<int> GetMessageCountForTaskAsync(int taskId)
    {
        return await context.TaskMessage
            .Where(tm => tm.RelatedTaskId == taskId)
            .CountAsync();
    }

    public async Task<int> CreateAsync(TaskMessageCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.MessageText))
            throw new ArgumentException("Message text cannot be empty");

        var message = new TaskMessage
        {
            MessageText = dto.MessageText,
            SenderId = dto.SenderId,
            RelatedTaskId = dto.RelatedTaskId,
            CreatedOn = DateTime.UtcNow
        };

        await context.TaskMessage.AddAsync(message);
        await context.SaveChangesAsync();

        return await context.SaveChangesAsync();
    }

    public async Task UpdateMessageTextAsync(int messageId, string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("Message text cannot be empty");

        var message = await context.TaskMessage.FindAsync(messageId);
        if (message != null)
        {
            message.MessageText = newText;
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var message = await context.TaskMessage.FindAsync(id);
        if (message != null)
        {
            context.TaskMessage.Remove(message);
            await context.SaveChangesAsync();
        }
    }
    
    public async Task<TaskMessage?> GetByIdAsync(int id)
    {
        var message = await context.TaskMessage.FindAsync(id);
        return message;
    }
}