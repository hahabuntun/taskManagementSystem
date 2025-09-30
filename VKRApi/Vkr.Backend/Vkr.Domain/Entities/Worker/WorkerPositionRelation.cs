namespace Vkr.Domain.Entities.Worker;

/// <summary>
/// Управление разрешениями должности на получение и постановку задач
/// </summary>
public class WorkerPositionRelation
{
    public int Id { get; set; }
    /// <summary>
    /// какая должность ставит задачи
    /// </summary>
    /// 
    public int WorkerPositionId { get; set; }
    /// <summary>
    /// кому ставит задачи
    /// </summary>
    public int SubordinateWorkerPositionId { get; set; }

    public WorkerPosition WorkerPosition { get; set; }
    public WorkerPosition SubordinateWorkerPosition { get; set; }
}