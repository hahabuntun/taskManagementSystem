namespace Vkr.Domain.Entities.Worker;

/// <summary>
/// Связь Начальник - Подчинённый
/// </summary>
public class WorkersManagement
{
    public int Id { get; set; }
    
    public int ManagerId { get; set; }
    
    public int SubordinateId { get; set; }
    
    public Workers Manager { get; set; }
    
    public Workers Subordinate { get; set; }
}