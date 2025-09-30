namespace Vkr.Domain.Entities.Worker;

/// <summary>
/// Должность работника. Она определяет привилегии
/// </summary>
public class WorkerPosition
{
    public const int MaxTitleLength = 50;
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>

    // Navigation property for Worker in this position
    public ICollection<Workers> Workers { get; set; } = new List<Workers>();
    
    // Navigation properties for WorkerPositionRelation
    public ICollection<WorkerPositionRelation> TaskGiverRelations { get; set; } = new List<WorkerPositionRelation>(); // Positions this position assigns tasks to
    public ICollection<WorkerPositionRelation> TaskTakerRelations { get; set; } = new List<WorkerPositionRelation>(); // Positions that assign tasks to this position
}