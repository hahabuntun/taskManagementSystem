namespace Vkr.Domain.Entities.Journal;

public class ChangeLog
{
    public int    Id           { get; set; }
    public string EntityName   { get; set; } = null!;
    public string? EntityKey   { get; set; }
    public string PropertyName { get; set; } = null!;
    public string? OldValue    { get; set; }
    public string? NewValue    { get; set; }
    public string ChangeType   { get; set; } = null!;
    public DateTime ChangedOn  { get; set; }
    public string? ChangedBy   { get; set; }
}