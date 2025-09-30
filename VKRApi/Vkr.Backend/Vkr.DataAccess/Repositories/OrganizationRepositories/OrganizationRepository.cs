using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.OrganizationRepositories;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Worker;

namespace Vkr.DataAccess.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganizationDTO>> GetOrganizationsAsync()
        {
            return await _context.Organizations
                .Include(o => o.Owner)
                    .ThenInclude(w => w.WorkerPosition)
                        .ThenInclude(wp => wp.TaskGiverRelations)
                            .ThenInclude(r => r.SubordinateWorkerPosition)
                .Include(o => o.Owner)
                    .ThenInclude(w => w.WorkerPosition)
                        .ThenInclude(wp => wp.TaskTakerRelations)
                            .ThenInclude(r => r.WorkerPosition)
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name ?? string.Empty,
                    CreatedOn = o.CreatedOn,
                    Owner = new WorkerDTO
                    {
                        Id = o.Owner.Id,
                        Name = o.Owner.Name,
                        SecondName = o.Owner.SecondName,
                        ThirdName = o.Owner.ThirdName,
                        Email = o.Owner.Email,
                        CreatedOn = o.Owner.CreatedOn,
                        Phone = o.Owner.Phone,
                        WorkerPositionId = o.Owner.WorkerPositionId,
                        WorkerPosition = new WorkerPositionDto
                        {
                            Id = o.Owner.WorkerPosition.Id,
                            Title = o.Owner.WorkerPosition.Title,
                            TaskGivers = o.Owner.WorkerPosition.TaskGiverRelations.Select(r => new WorkerPositionSummary
                            {
                                Id = r.SubordinateWorkerPosition.Id,
                                Title = r.SubordinateWorkerPosition.Title
                            }).ToList(),
                            TaskTakers = o.Owner.WorkerPosition.TaskTakerRelations.Select(r => new WorkerPositionSummary
                            {
                                Id = r.WorkerPosition.Id,
                                Title = r.WorkerPosition.Title
                            }).ToList()
                        },
                        CanManageWorkers = o.Owner.CanManageWorkers,
                        CanManageProjects = o.Owner.CanManageProjects,
                        WorkerStatus = new WorkerStatusDto
                        {
                            Id = (int)o.Owner.WorkerStatus,
                            Name = o.Owner.WorkerStatus.ToString()
                        }
                    }
                })
                .OrderBy(o => o.Name)
                .ToListAsync();
        }
    }
}