using Vkr.Domain.Entities.Progect;

namespace Vkr.Domain.Repositories
{
    public interface IProjectLinkRepository
    {
        /// <summary>
        /// Добавляет новую ссылку в базу данных.
        /// </summary>
        /// <param name="projectLink">Объект ссылки проекта.</param>
        /// <returns>Идентификатор созданной ссылки.</returns>
        Task<int> AddLinkAsync(ProjectLink projectLink);

        /// <summary>
        /// Обновляет существующую ссылку.
        /// </summary>
        /// <param name="linkId">Идентификатор ссылки.</param>
        /// <param name="link">Новый URL ссылки.</param>
        /// <param name="description">Новое описание ссылки (необязательно).</param>
        /// <returns>True, если ссылка успешно обновлена, иначе false.</returns>
        Task<bool> UpdateLinkAsync(int linkId, string link, string? description);

        /// <summary>
        /// Удаляет ссылку по её идентификатору.
        /// </summary>
        /// <param name="linkId">Идентификатор ссылки.</param>
        /// <returns>True, если ссылка успешно удалена, иначе false.</returns>
        Task<bool> DeleteLinkAsync(int linkId);

        /// <summary>
        /// Возвращает список ссылок для указанного проекта.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <returns>Список ссылок проекта.</returns>
        Task<IEnumerable<ProjectLink>> GetLinksByProjectIdAsync(int projectId);

        Task<ProjectLink?> GetLinkByIdAsync(int linkId);
    }
}