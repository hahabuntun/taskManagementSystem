using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Task;

public class TaskMessage
{
    public int Id;

    public string MessageText { get; set; } = string.Empty;
    
    public DateTime CreatedOn { get; set; }

    public int SenderId { get; set; }

    public Workers Sender { get; set; } = null!;
    
    public int RelatedTaskId { get; set; }

    public Tasks RelatedTask { get; set; } = null!;
}