namespace Vkr.Domain.Entities.Progect;

public class ProjectChecklistCheck
{
    public int Id { get; }

    /// <summary>
    /// Название
    /// </summary>
    public string? Tittle { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Выполнена?
    /// </summary>
    public bool IsChecked { get; set; }

    public int ProjectChecklistId { get; set; }

    /// <summary>
    /// Связь со списком
    /// </summary>
    public ProjectChecklist ProjectChecklist { get; set; }  = null!;
}