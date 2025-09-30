using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Organization;
using Vkr.Domain.Entities.Sprint;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Progect;

public class Projects
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Название 
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Цель
    /// </summary>
    public string? Goal { get; set; }
    
    /// <summary>
    /// Идентификатор управляющего
    /// </summary>
    public int ManagerId { get; set; }

    public int Progress { get; set; }

    /// <summary>
    /// Сущность управляющего
    /// </summary>
    public Workers Manager { get; set; } = null!;
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedOn { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Иденификатор организации в которой работает
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// Сущность организации
    /// </summary>
    public Organizations Organization { get; set; } = null!;
    
    public int? ProjectStatusId { get; set; }
    
    public ProjectStatus? ProjectStatus { get; set; }

    /// <summary>
    /// Навигация по ссылкам проекта
    /// </summary>
    public List<ProjectLink> ProjectLinks { get; } = [];
    
    /// <summary>
    /// Навигация по спискам проверок проекта
    /// </summary>
    public List<ProjectChecklist> ProjectChecklists { get; } = [];

    /// <summary>
    /// Навигация по тэгам
    /// </summary>
    public List<Tags> Tags { get; set; } = [];


    /// <summary>
    /// Навигация по тэгам
    /// </summary>
    public List<Tasks> Tasks { get; set; } = [];

    /// <summary>
    /// Работникик проекта
    /// </summary>
    public List<Workers> WorkersList { get; set; } = [];
    public List<Sprints> Sprints { get; set; } = [];
    public List<ProjectMemberManagement> ProjectMemberManagements { get; set; } = [];
    
    /// <summary>
    /// Доски проекта
    /// </summary>
    public List<Boards> BoardsProgect { get; set; }
}