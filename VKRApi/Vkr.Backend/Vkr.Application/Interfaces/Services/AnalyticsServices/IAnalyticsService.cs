using Vkr.Application.Services;

namespace Vkr.Application.Interfaces.Services;

public interface IAnalyticsService
{
    Task<ProjectAnalyticsData> GetProjectAnalyticsAsync(int projectId);
    Task<SprintAnalyticsData> GetSprintAnalyticsAsync(int sprintId);
    Task<WorkerAnalyticsData> GetWorkerAnalyticsAsync(int workerId);
    Task<OrganizationAnalyticsData> GetOrganizationAnalyticsAsync(int organizationId);
}