using Microsoft.EntityFrameworkCore;
using Vkr.Application.Filters;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Project;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.ProjectRepositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDTO?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.ProjectStatus)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Include(p => p.ProjectChecklists)
            .Include(p => p.Tags)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Where(p => p.Id == id)
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Goal = p.Goal ?? string.Empty,
                ManagerId = p.ManagerId,
                Manager = new WorkerDTO
                {
                    Id = p.Manager.Id,
                    Name = p.Manager.Name,
                    SecondName = p.Manager.SecondName,
                    ThirdName = p.Manager.ThirdName,
                    Email = p.Manager.Email,
                    CreatedOn = p.Manager.CreatedOn,
                    Phone = p.Manager.Phone,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)p.Manager.WorkerStatus,
                        Name = p.Manager.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = p.Manager.WorkerPosition.Id,
                        Title = p.Manager.WorkerPosition.Title,
                        TaskGivers = p.Manager.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = p.Manager.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                },
                Progress = p.Progress,
                CreatedOn = p.CreatedOn,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatusId = p.ProjectStatusId,
                ProjectStatusName = p.ProjectStatus != null ? p.ProjectStatus.Name : string.Empty,
                ProjectChecklists = p.ProjectChecklists,
                Tags = p.Tags,
                Workers = p.WorkersList.Select(w => new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerPositionId = w.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus,
                        Name = w.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        Title = w.WorkerPosition.Title,
                        TaskGivers = w.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = w.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.ProjectStatus)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Include(p => p.ProjectChecklists)
            .Include(p => p.Tags)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Goal = p.Goal ?? string.Empty,
                ManagerId = p.ManagerId,
                Manager = new WorkerDTO
                {
                    Id = p.Manager.Id,
                    Name = p.Manager.Name,
                    SecondName = p.Manager.SecondName,
                    ThirdName = p.Manager.ThirdName,
                    Email = p.Manager.Email,
                    CreatedOn = p.Manager.CreatedOn,
                    Phone = p.Manager.Phone,
                    WorkerPositionId = p.Manager.WorkerPositionId,
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = p.Manager.WorkerPosition.Id,
                        Title = p.Manager.WorkerPosition.Title,
                        TaskGivers = p.Manager.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = p.Manager.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    },
                    CanManageWorkers = p.Manager.CanManageWorkers,
                    CanManageProjects = p.Manager.CanManageProjects,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)p.Manager.WorkerStatus,
                        Name = p.Manager.WorkerStatus.ToString()
                    },
                },
                Progress = p.Progress,
                CreatedOn = p.CreatedOn,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatusId = p.ProjectStatusId,
                ProjectStatusName = p.ProjectStatus != null ? p.ProjectStatus.Name : string.Empty,
                ProjectChecklists = p.ProjectChecklists,
                Tags = p.Tags,
                Workers = p.WorkersList.Select(w => new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerPositionId = w.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus,
                        Name = w.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        Title = w.WorkerPosition.Title,
                        TaskGivers = w.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = w.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    },
                    CanManageWorkers = w.CanManageWorkers,
                    CanManageProjects = w.CanManageProjects,
                }).ToList()
            })
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectDTO>> GetByOrganizationAsync(int organizationId)
    {
        return await _context.Projects
            .Where(p => p.OrganizationId == organizationId)
            .Include(p => p.ProjectStatus)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Include(p => p.ProjectChecklists)
            .Include(p => p.Tags)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Goal = p.Goal ?? string.Empty,
                ManagerId = p.ManagerId,
                Manager = new WorkerDTO
                {
                    Id = p.Manager.Id,
                    Name = p.Manager.Name,
                    SecondName = p.Manager.SecondName,
                    ThirdName = p.Manager.ThirdName,
                    Email = p.Manager.Email,
                    CreatedOn = p.Manager.CreatedOn,
                    Phone = p.Manager.Phone,
                    WorkerPositionId = p.Manager.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)p.Manager.WorkerStatus,
                        Name = p.Manager.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = p.Manager.WorkerPosition.Id,
                        Title = p.Manager.WorkerPosition.Title,
                        TaskGivers = p.Manager.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = p.Manager.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                },
                Progress = p.Progress,
                CreatedOn = p.CreatedOn,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatusId = p.ProjectStatusId,
                ProjectStatusName = p.ProjectStatus != null ? p.ProjectStatus.Name : string.Empty,
                ProjectChecklists = p.ProjectChecklists,
                Tags = p.Tags,
                Workers = p.WorkersList.Select(w => new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerPositionId = w.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus,
                        Name = w.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        Title = w.WorkerPosition.Title,
                        TaskGivers = w.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = w.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                }).ToList()
            })
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectDTO>> GetByStatusAsync(int statusId)
    {
        return await _context.Projects
            .Where(p => p.ProjectStatusId == statusId)
            .Include(p => p.ProjectStatus)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Include(p => p.ProjectChecklists)
            .Include(p => p.Tags)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Goal = p.Goal ?? string.Empty,
                ManagerId = p.ManagerId,
                Manager = new WorkerDTO
                {
                    Id = p.Manager.Id,
                    Name = p.Manager.Name,
                    SecondName = p.Manager.SecondName,
                    ThirdName = p.Manager.ThirdName,
                    Email = p.Manager.Email,
                    CreatedOn = p.Manager.CreatedOn,
                    Phone = p.Manager.Phone,
                    WorkerPositionId = p.Manager.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)p.Manager.WorkerStatus,
                        Name = p.Manager.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = p.Manager.WorkerPosition.Id,
                        Title = p.Manager.WorkerPosition.Title,
                        TaskGivers = p.Manager.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = p.Manager.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                },
                Progress = p.Progress,
                CreatedOn = p.CreatedOn,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatusId = p.ProjectStatusId,
                ProjectStatusName = p.ProjectStatus != null ? p.ProjectStatus.Name : string.Empty,
                ProjectChecklists = p.ProjectChecklists,
                Tags = p.Tags,
                Workers = p.WorkersList.Select(w => new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerPositionId = w.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus,
                        Name = w.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        Title = w.WorkerPosition.Title,
                        TaskGivers = w.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = w.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                }).ToList()
            })
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectDTO>> GetByManagerAsync(int managerId)
    {
        return await _context.Projects
            .Where(p => p.ManagerId == managerId)
            .Include(p => p.ProjectStatus)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Include(p => p.ProjectChecklists)
            .Include(p => p.Tags)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                        .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                        .ThenInclude(r => r.WorkerPosition)
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Goal = p.Goal ?? string.Empty,
                ManagerId = p.ManagerId,
                Manager = new WorkerDTO
                {
                    Id = p.Manager.Id,
                    Name = p.Manager.Name,
                    SecondName = p.Manager.SecondName,
                    ThirdName = p.Manager.ThirdName,
                    Email = p.Manager.Email,
                    CreatedOn = p.Manager.CreatedOn,
                    Phone = p.Manager.Phone,
                    WorkerPositionId = p.Manager.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)p.Manager.WorkerStatus,
                        Name = p.Manager.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = p.Manager.WorkerPosition.Id,
                        Title = p.Manager.WorkerPosition.Title,
                        TaskGivers = p.Manager.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = p.Manager.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                },
                Progress = p.Progress,
                CreatedOn = p.CreatedOn,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatusId = p.ProjectStatusId,
                ProjectStatusName = p.ProjectStatus != null ? p.ProjectStatus.Name : string.Empty,
                ProjectChecklists = p.ProjectChecklists,
                Tags = p.Tags,
                Workers = p.WorkersList.Select(w => new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerPositionId = w.WorkerPositionId,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus,
                        Name = w.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        Title = w.WorkerPosition.Title,
                        TaskGivers = w.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.SubordinateWorkerPosition.Id,
                            Title = r.SubordinateWorkerPosition.Title
                        }).ToList(),
                        TaskTakers = w.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                        {
                            Id = r.WorkerPosition.Id,
                            Title = r.WorkerPosition.Title
                        }).ToList()
                    }
                }).ToList()
            })
            .OrderByDescending(p => p.CreatedOn)
            .ToListAsync();
    }

    public async Task<bool> ProjectNameExistsAsync(string name, int organizationId)
    {
        return await _context.Projects
            .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.OrganizationId == organizationId);
    }

    public async Task<int> AddAsync(ProjectCreateDTO project)
    {
        ArgumentNullException.ThrowIfNull(project);

        if (string.IsNullOrWhiteSpace(project.Name))
            throw new ArgumentException("Project name is required");

        if (await ProjectNameExistsAsync(project.Name, project.OrganizationId))
            throw new InvalidOperationException("Project name must be unique within organization");

        var manager = await _context.Workers
            .FirstOrDefaultAsync(w => w.Id == project.ManagerId)
            ?? throw new InvalidOperationException($"Manager with ID {project.ManagerId} not found");

        var workers = await _context.Workers
            .Where(w => project.Members.Contains(w.Id) && w.Id != project.ManagerId)
            .ToListAsync();

        var newProject = new Projects
        {
            Name = project.Name,
            Goal = project.Goal,
            Description = project.Description,
            ManagerId = project.ManagerId,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Progress = project.Progress,
            OrganizationId = project.OrganizationId,
            ProjectStatusId = project.ProjectStatusId,
            WorkersList = workers,
            Tags = [] // Tags are handled by TagRepository
        };

        newProject.WorkersList.Add(manager);

        await _context.Projects.AddAsync(newProject);
        await _context.SaveChangesAsync();

        return newProject.Id;
    }

    public async Task UpdateAsync(int projectId, ProjectUpdateDto projectDto)
    {
        ArgumentNullException.ThrowIfNull(projectDto);

        var project = await _context.Projects
            .Include(p => p.WorkersList)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new KeyNotFoundException($"Project with ID {projectId} not found");

        if (string.IsNullOrWhiteSpace(projectDto.Name))
            throw new ArgumentException("Project name is required");

        project.Name = projectDto.Name;
        project.Description = projectDto.Description;
        project.Goal = projectDto.Goal;
        project.ProjectStatusId = projectDto.ProjectStatusId;
        project.Progress = projectDto.Progress;
        project.StartDate = projectDto.StartDate;
        project.EndDate = projectDto.EndDate;

        // Update workers
        if (projectDto.Members != null)
        {
            var workers = await _context.Workers
                .Where(w => projectDto.Members.Contains(w.Id) && w.Id != project.ManagerId)
                .ToListAsync();

            project.WorkersList.Clear();
            project.WorkersList.AddRange(workers);
            if (!project.WorkersList.Any(w => w.Id == project.ManagerId))
            {
                var manager = await _context.Workers
                    .FirstOrDefaultAsync(w => w.Id == project.ManagerId)
                    ?? throw new InvalidOperationException($"Manager with ID {project.ManagerId} not found");
                project.WorkersList.Add(manager);
            }
        }

        // Tags are handled by TagRepository
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (entity != null)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ProjectDTO>> GetByFilterAsync(ProjectsFilter filter)
    {
        var query = _context.Projects
            .Include(p => p.ProjectStatus)
            .Include(p => p.Manager)
                .ThenInclude(m => m.WorkerPosition)
            .Include(p => p.WorkersList)
                .ThenInclude(w => w.WorkerPosition)
            .Include(p => p.Tags)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name));
        }

        if (filter.StatusId.HasValue)
        {
            query = query.Where(p => p.ProjectStatusId == filter.StatusId.Value);
        }

        if (filter.ManagerId.HasValue)
        {
            query = query.Where(p => p.ManagerId == filter.ManagerId.Value);
        }

        if (filter.StartDateFrom.HasValue)
        {
            query = query.Where(p => p.StartDate >= filter.StartDateFrom.Value);
        }

        if (filter.StartDateTill.HasValue)
        {
            query = query.Where(p => p.StartDate <= filter.StartDateTill.Value);
        }

        if (filter.EndDateFrom.HasValue)
        {
            query = query.Where(p => p.EndDate >= filter.EndDateFrom.Value);
        }

        if (filter.EndDateTill.HasValue)
        {
            query = query.Where(p => p.EndDate <= filter.EndDateTill.Value);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(p => p.CreatedOn >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTill.HasValue)
        {
            query = query.Where(p => p.CreatedOn <= filter.CreatedTill.Value);
        }

        return await query
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Goal = p.Goal ?? string.Empty,
                ManagerId = p.ManagerId,
                Manager = new WorkerDTO
                {
                    Id = p.Manager.Id,
                    Name = p.Manager.Name,
                    SecondName = p.Manager.SecondName,
                    ThirdName = p.Manager.ThirdName,
                    Email = p.Manager.Email,
                    CreatedOn = p.Manager.CreatedOn,
                    Phone = p.Manager.Phone,
                    WorkerPositionId = p.Manager.WorkerPositionId,
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = p.Manager.WorkerPosition.Id,
                        Title = p.Manager.WorkerPosition.Title
                    },
                    CanManageWorkers = p.Manager.CanManageWorkers,
                    CanManageProjects = p.Manager.CanManageProjects,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)p.Manager.WorkerStatus,
                        Name = p.Manager.WorkerStatus.ToString()
                    },
                },
                Progress = p.Progress,
                CreatedOn = p.CreatedOn,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatusId = p.ProjectStatusId,
                ProjectStatusName = p.ProjectStatus != null ? p.ProjectStatus.Name : string.Empty,
                Tags = p.Tags,
                Workers = p.WorkersList.Select(w => new WorkerDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    SecondName = w.SecondName,
                    ThirdName = w.ThirdName,
                    Email = w.Email,
                    CreatedOn = w.CreatedOn,
                    Phone = w.Phone,
                    WorkerStatus = new WorkerStatusDto
                    {
                        Id = (int)w.WorkerStatus,
                        Name = w.WorkerStatus.ToString()
                    },
                    WorkerPosition = new WorkerPositionDto
                    {
                        Id = w.WorkerPosition.Id,
                        Title = w.WorkerPosition.Title
                    },
                    CanManageWorkers = w.CanManageWorkers,
                    CanManageProjects = w.CanManageProjects,
                }).ToList()
            })
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksByProjectIdAsync(int projectId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Sprint)
            .Include(t => t.Creator)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .ToListAsync();

        var relationshipTypes = await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return tasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }
    private TaskDTO MapToTaskDTO(Tasks task, Dictionary<int, string> relationshipTypes)
    {
        var relatedTasks = new List<RelatedTaskDTO>();

        relatedTasks.AddRange(task.TaskRelationships.Select(tr => new RelatedTaskDTO
        {
            Task = MapToMinimalTaskDTO(tr.RelatedTask),
            RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name) ? name : "Unknown",
            RelationshipId = tr.Id
        }));

        relatedTasks.AddRange(task.RelatedTaskRelationships.Select(tr => new RelatedTaskDTO
        {
            Task = MapToMinimalTaskDTO(tr.Task),
            RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name)
                ? (name == "ParentChild" ? "Parent" : name)
                : "Unknown",
            RelationshipId = tr.Id
        }));

        var responsibleExecutor = task.TaskExecutors.FirstOrDefault(te => te.IsResponsible);

        if (responsibleExecutor == null && task.TaskExecutors.Any())
        {
            Console.WriteLine($"Warning: No responsible executor found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Select(te => te.WorkerId))}");
        }

        var responsibleCount = task.TaskExecutors.Count(te => te.IsResponsible);
        if (responsibleCount > 1)
        {
            Console.WriteLine($"Error: Multiple responsible executors found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Where(te => te.IsResponsible).Select(te => te.WorkerId))}");
            throw new InvalidOperationException("Multiple responsible executors detected for task. Only one is allowed.");
        }

        return new TaskDTO
        {
            Id = task.Id,
            ShortName = task.ShortName ?? string.Empty,
            Description = task.Description,
            CreatedOn = task.CreatedOn,
            Progress = task.Progress,
            StartOn = task.StartOn,
            ExpireOn = task.ExpireOn,
            StoryPoints = task.StoryPoints,
            Project = new ProjectDTO { Id = task.Project.Id, Name = task.Project.Name },
            Creator = new WorkerDTO { Id = task.Creator.Id, Name = task.Creator.Name },
            TaskType = new TaskTypeDTO { Id = task.TaskType.Id, Name = task.TaskType.Name },
            TaskStatus = new TaskStatusDTO { Id = task.TaskStatus.Id, Name = task.TaskStatus.Name, Color = task.TaskStatus.Color },
            TaskPriority = task.TaskPriority != null ? new TaskPriorityDTO { Id = task.TaskPriority.Id, Name = task.TaskPriority.Name, Color = task.TaskPriority.Color } : null,
            Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
            Executors = task.TaskExecutors.Select(te => new WorkerDTO
            {
                Id = te.Worker.Id,
                Name = te.Worker.Name,
                SecondName = te.Worker.SecondName,
                Email = te.Worker.Email,
                CreatedOn = te.Worker.CreatedOn,
                WorkerPositionId = te.Worker.WorkerPositionId,
                CanManageWorkers = te.Worker.CanManageWorkers,
                CanManageProjects = te.Worker.CanManageProjects
            }).ToList(),
            Observers = task.TaskObservers.Select(to => new WorkerDTO
            {
                Id = to.Worker.Id,
                Name = to.Worker.Name,
                SecondName = to.Worker.SecondName,
                Email = to.Worker.Email,
                CreatedOn = to.Worker.CreatedOn,
                WorkerPositionId = to.Worker.WorkerPositionId,
                CanManageWorkers = to.Worker.CanManageWorkers,
                CanManageProjects = to.Worker.CanManageProjects
            }).ToList(),
            ResponsibleWorker = responsibleExecutor != null ? new WorkerDTO
            {
                Id = responsibleExecutor.Worker.Id,
                Name = responsibleExecutor.Worker.Name,
                SecondName = responsibleExecutor.Worker.SecondName,
                Email = responsibleExecutor.Worker.Email,
                CreatedOn = responsibleExecutor.Worker.CreatedOn,
                WorkerPositionId = responsibleExecutor.Worker.WorkerPositionId,
                CanManageWorkers = responsibleExecutor.Worker.CanManageWorkers,
                CanManageProjects = responsibleExecutor.Worker.CanManageProjects
            } : null,
            RelatedTasks = relatedTasks,
            TagDTOs = task.Tags.Select(t => new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color }).ToList(),
        };
    }

    private TaskDTO MapToMinimalTaskDTO(Tasks task)
    {
        return new TaskDTO
        {
            Id = task.Id,
            ShortName = task.ShortName ?? string.Empty,
            Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
            StoryPoints = task.StoryPoints,
            Project = new ProjectDTO
            {
                Id = task.Project.Id,
                Name = task.Project.Name,
                Description = task.Project.Description,
                Goal = task.Project.Goal,
                ManagerId = task.Project.ManagerId,
                Progress = task.Project.Progress,
                ProjectStatusName = task.Project.ProjectStatus?.Name,
            },
            RelatedTasks = new List<RelatedTaskDTO>()
        };
    }
}