namespace Vkr.Application.Interfaces.Repositories.AnalyticsRepositories;

public interface IAnalyticsRepository
{
    Task<OrganizationAnalyticsData> GetOrganizationAnalyticsAsync(int organizationId);
    Task<WorkerAnalyticsData> GetWorkerAnalyticsAsync(int workerId);
    Task<SprintAnalyticsData> GetSprintAnalyticsAsync(int sprintId);
    Task<ProjectAnalyticsData> GetProjectAnalyticsAsync(int projectId);
}