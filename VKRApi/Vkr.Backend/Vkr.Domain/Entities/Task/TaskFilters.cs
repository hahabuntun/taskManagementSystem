namespace Vkr.Domain.Entities;

public class TaskFilter
{
    public string Name { get; set; } = string.Empty; // Primary key
    public string OptionsJson { get; set; } = string.Empty; // JSON-serialized ITaskFilterOptions
}