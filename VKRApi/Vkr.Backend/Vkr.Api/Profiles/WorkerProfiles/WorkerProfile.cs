using AutoMapper;
using Vkr.API.Contracts.WorkerPositionsControllerContracts;
using Vkr.API.Contracts.WorkersControllerContracts;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Profiles.WorkerProfiles
{
    public class WorkerProfile : Profile
    {
        public WorkerProfile()
        {
            // Map CreateWorkerRequest to Workers (entity)
            CreateMap<CreateWorkerRequest, Workers>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.SecondName, src => src.MapFrom(x => x.SecondName))
                .ForMember(dest => dest.ThirdName, src => src.MapFrom(x => x.ThirdName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.CanManageProjects, src => src.MapFrom(x => x.CanManageProjects))
                .ForMember(dest => dest.CanManageWorkers, src => src.MapFrom(x => x.CanManageWorkers))
                .ForMember(dest => dest.WorkerStatus, src => src.MapFrom(x => x.WorkerStatus))
                .ForMember(dest => dest.WorkerPositionId, src => src.MapFrom(x => x.WorkerPosition))
                .ForMember(dest => dest.WorkerPosition, src => src.Ignore());

            // Map UpdateWorkerRequest to Workers (entity)
            CreateMap<UpdateWorkerRequest, Workers>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.SecondName, src => src.MapFrom(x => x.SecondName))
                .ForMember(dest => dest.ThirdName, src => src.MapFrom(x => x.ThirdName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.CanManageProjects, src => src.MapFrom(x => x.CanManageProjects))
                .ForMember(dest => dest.CanManageWorkers, src => src.MapFrom(x => x.CanManageWorkers))
                .ForMember(dest => dest.WorkerStatus, src => src.MapFrom(x => x.WorkerStatus))
                .ForMember(dest => dest.WorkerPositionId, src => src.MapFrom(x => x.WorkerPosition))
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.WorkerPosition, src => src.Ignore())
                .ForMember(dest => dest.PasswordHash, src => src.Ignore())
                .ForMember(dest => dest.NormalizedEmail, src => src.MapFrom(x => x.Email));

            // Map Workers (entity) to WorkerResponse (DTO)
            CreateMap<Workers, WorkerResponse>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.SecondName, src => src.MapFrom(x => x.SecondName))
                .ForMember(dest => dest.ThirdName, src => src.MapFrom(x => x.ThirdName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.CanManageProjects, src => src.MapFrom(x => x.CanManageProjects))
                .ForMember(dest => dest.CanManageWorkers, src => src.MapFrom(x => x.CanManageWorkers))
                .ForMember(dest => dest.WorkerStatus, src => src.MapFrom(x => new WorkerStatusResponse((int)x.WorkerStatus)))
                .ForMember(dest => dest.WorkerPosition, src => src.MapFrom(x => x.WorkerPosition))
                .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedOn));

            // Map WorkerPosition (entity) to WorkerPositionResponse (DTO)
            CreateMap<WorkerPosition, WorkerPositionResponse>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Title, src => src.MapFrom(x => x.Title))
                .ForMember(dest => dest.TaskGivers, src => src.MapFrom(x => x.TaskTakerRelations.Select(r => r.WorkerPosition)))
                .ForMember(dest => dest.TaskTakers, src => src.MapFrom(x => x.TaskGiverRelations.Select(r => r.SubordinateWorkerPosition)));

            // Map WorkerPosition (entity) to WorkerPositionSummary (DTO)
            CreateMap<WorkerPosition, WorkerPositionSummary>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Title, src => src.MapFrom(x => x.Title));
        }
    }
}