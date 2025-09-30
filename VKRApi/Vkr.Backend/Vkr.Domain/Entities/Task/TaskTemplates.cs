using System;
using System.Collections.Generic;
using Vkr.Domain.Entities;

namespace Vkr.Domain.Entities.Task;

public class TaskTemplates
{
    public int Id { get; set; }
    
    public string TemplateName { get; set; } = string.Empty;
    
    public string? TaskName { get; set; }
    
    public string? Description { get; set; }
    
    public int? TaskStatusId { get; set; }
    
    public TaskStatus? TaskStatus { get; set; }
    
    public int? TaskPriorityId { get; set; }
    
    public TaskPriority? TaskPriority { get; set; }
    
    public int? TaskTypeId { get; set; } // Foreign key for TaskType
    public TaskType? TaskType { get; set; } // Navigation property
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public DateTime? CreatedOn { get; set; }
    
    public int? Progress { get; set; }
    
    public int? StoryPoints { get; set; }
    
    public List<Tags> Tags { get; set; } = new List<Tags>();
    
    public List<TaskTemplateLink> TaskTempateLinks { get; set; } = new List<TaskTemplateLink>();
}