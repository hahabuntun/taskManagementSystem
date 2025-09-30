using System.Threading.Tasks;
using Vkr.Application.Interfaces.Repositories.AnalyticsRepositories;
using Vkr.Application.Interfaces.Services;
using Vkr.Application.Interfaces.Services.ProjectServices;
using Vkr.Application.Interfaces.Services.SprintServices;
using Vkr.Application.Interfaces.Services.TaskServices;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Worker;

namespace Vkr.Application.Services.AnalyticsServices;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsRepository _analyticsRepository;

    public AnalyticsService(
        IAnalyticsRepository analyticsRepository)
    {
        _analyticsRepository = analyticsRepository;
    }

    public async Task<OrganizationAnalyticsData> GetOrganizationAnalyticsAsync(int organizationId)
    {
        return await _analyticsRepository.GetOrganizationAnalyticsAsync(organizationId);
    }

    public async Task<WorkerAnalyticsData> GetWorkerAnalyticsAsync(int workerId)
    {
        return await _analyticsRepository.GetWorkerAnalyticsAsync(workerId);
    }

    public async Task<SprintAnalyticsData> GetSprintAnalyticsAsync(int sprintId)
    {
        return await _analyticsRepository.GetSprintAnalyticsAsync(sprintId);
    }

    public async Task<ProjectAnalyticsData> GetProjectAnalyticsAsync(int projectId)
    {
        return await _analyticsRepository.GetProjectAnalyticsAsync(projectId);
    }
}