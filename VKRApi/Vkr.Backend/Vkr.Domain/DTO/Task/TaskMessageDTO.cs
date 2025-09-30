namespace Vkr.Domain.DTO.Task;

public class TaskMessageDTO
{
    public int Id { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public int SenderId { get; set; }
    public string SenderEmail { get; set; }
    public string SenderSecondName { get; set; }
    public string SenderThirdName { get; set; }
    public string SenderName { get; set; } = string.Empty; 
    public int RelatedTaskId { get; set; }
}