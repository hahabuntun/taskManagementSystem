using Microsoft.EntityFrameworkCore;
using Vkr.Domain.Entities.Worker;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.DTO.Worker;

namespace Vkr.DataAccess.Repositories.ProjectRepositories;

public class ProjectMemberManagementRepository : IProjectMemberManagementRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectMemberManagementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectMemberDTO>> GetAllMembersAsync(int projectId)
{
    var members = await _context.Projects
        .Where(p => p.Id == projectId)
        .Include(p => p.WorkersList)
        .SelectMany(p => p.WorkersList)
        .Distinct()
        .Select(w => new ProjectMemberDTO
        {
            Worker = new WorkerDTO
            {
                Id = w.Id,
                Name = w.Name,
                SecondName = w.SecondName,
                ThirdName = w.ThirdName,
                Email = w.Email,
                CreatedOn = w.CreatedOn,
                Phone = w.Phone,
                WorkerPositionId = w.WorkerPositionId,
                CanManageWorkers = w.CanManageWorkers,
                CanManageProjects = w.CanManageProjects,
                WorkerPosition = new WorkerPositionDto
                {
                    Id = w.WorkerPosition.Id,
                    TaskGivers = new List<WorkerPositionSummary>(),
                    TaskTakers = new List<WorkerPositionSummary>(),
                    Title = w.WorkerPosition.Title
                },
                WorkerStatus = new WorkerStatusDto
                {
                    Id = (int)w.WorkerStatus ,
                    Name = w.WorkerStatus.ToString()
                }
            },
            TaskGivers = _context.ProjectMemberManagements
                .Where(pmm => pmm.ProjectId == projectId && pmm.SubordinateId == w.Id)
                .Select(pmm => new WorkerDTO
                {
                    Id = pmm.Worker.Id,
                    Name = pmm.Worker.Name,
                    SecondName = pmm.Worker.SecondName,
                    ThirdName = pmm.Worker.ThirdName,
                    Email = pmm.Worker.Email,
                    CreatedOn = pmm.Worker.CreatedOn,
                    Phone = pmm.Worker.Phone,
                    WorkerPositionId = pmm.Worker.WorkerPositionId,
                    CanManageWorkers = pmm.Worker.CanManageWorkers,
                    CanManageProjects = pmm.Worker.CanManageProjects,
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        TaskGivers = new List<WorkerPositionSummary>(),
                        TaskTakers = new List<WorkerPositionSummary>(),
                        Title = w.WorkerPosition.Title
                    },
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)pmm.Worker.WorkerStatus ,
                        Name = pmm.Worker.WorkerStatus.ToString()
                    }
                })
                .ToList(),
            TaskTakers = _context.ProjectMemberManagements
                .Where(pmm => pmm.ProjectId == projectId && pmm.WorkerId == w.Id)
                .Select(pmm => new WorkerDTO
                {
                    Id = pmm.Subordinate.Id,
                    Name = pmm.Subordinate.Name,
                    SecondName = pmm.Subordinate.SecondName,
                    ThirdName = pmm.Subordinate.ThirdName,
                    Email = pmm.Subordinate.Email,
                    CreatedOn = pmm.Subordinate.CreatedOn,
                    Phone = pmm.Subordinate.Phone,
                    WorkerPositionId = pmm.Subordinate.WorkerPositionId,
                    CanManageWorkers = pmm.Subordinate.CanManageWorkers,
                    CanManageProjects = pmm.Subordinate.CanManageProjects,
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        TaskGivers = new List<WorkerPositionSummary>(),
                        TaskTakers = new List<WorkerPositionSummary>(),
                        Title = w.WorkerPosition.Title
                    },
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)pmm.Subordinate.WorkerStatus ,
                        Name = pmm.Subordinate.WorkerStatus.ToString()
                    }
                })
                .ToList(),
            CreatedAt = w.CreatedOn
        })
        .ToListAsync();

    return members;
}

    public async Task<ProjectMemberDTO> GetMemberAsync(int projectId, int memberId)
    {
        var member = await _context.Projects
            .Where(p => p.Id == projectId)
            .Include(p => p.WorkersList)
            .SelectMany(p => p.WorkersList)
            .Where(w => w.Id == memberId)
            .Select(w => new ProjectMemberDTO
            {
                Worker = new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerPositionId = w.WorkerPositionId,
                    CanManageWorkers = w.CanManageWorkers,
                    CanManageProjects = w.CanManageProjects,
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        TaskGivers = new List<WorkerPositionSummary>(),
                        TaskTakers = new List<WorkerPositionSummary>(),
                        Title = w.WorkerPosition.Title
                    },
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus ,
                        Name = w.WorkerStatus.ToString()
                    }
                },
                TaskGivers = _context.ProjectMemberManagements
                    .Where(pmm => pmm.ProjectId == projectId && pmm.SubordinateId == w.Id)
                    .Select(pmm => new WorkerDTO
                    {
                        Id = pmm.Worker.Id,
                        Name = pmm.Worker.Name,
                        SecondName = pmm.Worker.SecondName,
                        ThirdName = pmm.Worker.ThirdName,
                        Email = pmm.Worker.Email,
                        CreatedOn = pmm.Worker.CreatedOn,
                        Phone = pmm.Worker.Phone,
                        WorkerPositionId = pmm.Worker.WorkerPositionId,
                        CanManageWorkers = pmm.Worker.CanManageWorkers,
                        CanManageProjects = pmm.Worker.CanManageProjects,
                        WorkerPosition = new WorkerPositionDto
                        {
                            Id = w.WorkerPosition.Id,
                            TaskGivers = new List<WorkerPositionSummary>(),
                            TaskTakers = new List<WorkerPositionSummary>(),
                            Title = w.WorkerPosition.Title
                        },
                        WorkerStatus = new WorkerStatusDto
                        {
                            Id = (int)pmm.Worker.WorkerStatus ,
                            Name = pmm.Worker.WorkerStatus.ToString()
                        }
                    })
                    .ToList(),
                TaskTakers = _context.ProjectMemberManagements
                    .Where(pmm => pmm.ProjectId == projectId && pmm.WorkerId == w.Id)
                    .Select(pmm => new WorkerDTO
                    {
                        Id = pmm.Subordinate.Id,
                        Name = pmm.Subordinate.Name,
                        SecondName = pmm.Subordinate.SecondName,
                        ThirdName = pmm.Subordinate.ThirdName,
                        Email = pmm.Subordinate.Email,
                        CreatedOn = pmm.Subordinate.CreatedOn,
                        Phone = pmm.Subordinate.Phone,
                        WorkerPositionId = pmm.Subordinate.WorkerPositionId,
                        CanManageWorkers = pmm.Subordinate.CanManageWorkers,
                        CanManageProjects = pmm.Subordinate.CanManageProjects,
                        WorkerPosition = new WorkerPositionDto
                        {
                            Id = w.WorkerPosition.Id,
                            TaskGivers = new List<WorkerPositionSummary>(),
                            TaskTakers = new List<WorkerPositionSummary>(),
                            Title = w.WorkerPosition.Title
                        },
                        WorkerStatus = new WorkerStatusDto
                        {
                            Id = (int)pmm.Subordinate.WorkerStatus ,
                            Name = pmm.Subordinate.WorkerStatus.ToString()
                        }
                    })
                    .ToList(),
                CreatedAt = w.CreatedOn
            })
            .FirstOrDefaultAsync();

        return member;
    }

    public async Task<IEnumerable<Workers>> GetMemberSubordinatesAsync(int projectId, int memberId)
    {
        var subordinates = await _context.ProjectMemberManagements
            .Where(pmm => pmm.ProjectId == projectId && pmm.WorkerId == memberId)
            .Include(pmm => pmm.Subordinate)
            .Select(pmm => pmm.Subordinate)
            .ToListAsync();

        return subordinates;
    }

    public async Task<IEnumerable<Workers>> GetMemberDirectors(int projectId, int memberId)
    {
        var directors = await _context.ProjectMemberManagements
            .Where(pmm => pmm.ProjectId == projectId && pmm.SubordinateId == memberId)
            .Include(pmm => pmm.Worker)
            .Select(pmm => pmm.Worker)
            .ToListAsync();

        return directors;
    }

    public async Task<bool> AddSubordinateToMember(int projectId, int memberId, int subordinateId)
    {
        var existingRelation = await _context.ProjectMemberManagements
            .AnyAsync(pmm => pmm.ProjectId == projectId &&
                            pmm.WorkerId == memberId &&
                            pmm.SubordinateId == subordinateId);

        if (existingRelation)
            return false;

        var newRelation = new ProjectMemberManagement
        {
            ProjectId = projectId,
            WorkerId = memberId,
            SubordinateId = subordinateId
        };

        await _context.ProjectMemberManagements.AddAsync(newRelation);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveSubordinateFromMember(int projectId, int memberId, int subordinateId)
    {
        var relation = await _context.ProjectMemberManagements
            .FirstOrDefaultAsync(pmm => pmm.ProjectId == projectId &&
                                      pmm.WorkerId == memberId &&
                                      pmm.SubordinateId == subordinateId);

        if (relation == null)
            return false;

        _context.ProjectMemberManagements.Remove(relation);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddProjectMember(int projectId, int memberId)
    {
        var relation = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);



        return true;
    }

    public async Task<bool> RemoveProjectMember(int projectId, int memberId)
    {
        var relation = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);



        return true;
    }

    public async Task<bool> AddMember(int projectId, int memberId)
    {
        // Находим проект с включением списка текущих работников
        var project = await _context.Projects
            .Include(p => p.WorkersList)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            return false;

        // Проверяем, существует ли работник
        var worker = await _context.Workers
            .Include(w => w.WorkerPosition)
            .FirstOrDefaultAsync(w => w.Id == memberId);

        if (worker == null)
            return false;

        // Проверяем, не является ли работник уже членом проекта
        if (project.WorkersList.Any(w => w.Id == memberId))
            return false;

        // Добавляем работника в список проекта
        project.WorkersList.Add(worker);

        // Находим все связи должностей для новой должности
        var workerPositionId = worker.WorkerPositionId;
        var positionRelations = await _context.WorkerPositionRelations
            .Where(wpr => wpr.WorkerPositionId == workerPositionId || wpr.SubordinateWorkerPositionId == workerPositionId)
            .ToListAsync();

        // Получаем всех текущих работников проекта с их должностями
        var existingWorkers = await _context.Projects
            .Where(p => p.Id == projectId)
            .Include(p => p.WorkersList)
            .SelectMany(p => p.WorkersList)
            .Include(w => w.WorkerPosition)
            .ToListAsync();

        // Создаем список новых отношений для ProjectMemberManagements
        var newRelations = new List<ProjectMemberManagement>();

        foreach (var existingWorker in existingWorkers)
        {
            if (existingWorker.Id == memberId)
                continue; // Пропускаем самого нового работника

            // Проверяем, может ли новый работник ставить задачи существующему (новый работник -> TaskGiver)
            var canAssignTo = positionRelations.FirstOrDefault(wpr =>
                wpr.WorkerPositionId == workerPositionId &&
                wpr.SubordinateWorkerPositionId == existingWorker.WorkerPositionId);

            if (canAssignTo != null)
            {
                newRelations.Add(new ProjectMemberManagement
                {
                    ProjectId = projectId,
                    WorkerId = memberId, // Новый работник ставит задачи
                    SubordinateId = existingWorker.Id // Существующий работник получает задачи
                });
            }

            // Проверяем, может ли существующий работник ставить задачи новому (новый работник -> TaskTaker)
            var canReceiveFrom = positionRelations.FirstOrDefault(wpr =>
                wpr.WorkerPositionId == existingWorker.WorkerPositionId &&
                wpr.SubordinateWorkerPositionId == workerPositionId);

            if (canReceiveFrom != null)
            {
                newRelations.Add(new ProjectMemberManagement
                {
                    ProjectId = projectId,
                    WorkerId = existingWorker.Id, // Существующий работник ставит задачи
                    SubordinateId = memberId // Новый работник получает задачи
                });
            }
        }

        // Добавляем новые отношения в ProjectMemberManagements
        await _context.ProjectMemberManagements.AddRangeAsync(newRelations);

        // Сохраняем изменения
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveMember(int projectId, int memberId)
    {
        var project = await _context.Projects
        .Include(p => p.WorkersList)
        .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            return false;

        // Проверяем, является ли работник членом проекта
        var worker = project.WorkersList.FirstOrDefault(w => w.Id == memberId);
        if (worker == null)
            return false;

        // Удаляем работника из списка проекта
        project.WorkersList.Remove(worker);

        // Сохраняем изменения
        await _context.SaveChangesAsync();
        return true;
    }
}