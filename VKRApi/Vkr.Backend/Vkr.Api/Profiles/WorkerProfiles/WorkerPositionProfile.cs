using AutoMapper;
using Vkr.API.Contracts.WorkerPositionsControllerContracts;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Profiles.WorkerProfiles;

public class WorkerPositionProfile : Profile
{
    public WorkerPositionProfile()
    {
        CreateMap<WorkerPositionCreateUpdateRequest, WorkerPosition>()
            .ForMember(dest => dest.Title, src => src.MapFrom(x => x.Title))
            .ForMember(dest => dest.Workers, src => src.Ignore())
            .ForMember(dest => dest.TaskGiverRelations, src => src.Ignore())
            .ForMember(dest => dest.TaskTakerRelations, src => src.Ignore());

        CreateMap<WorkerPosition, WorkerPositionDto>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.Title, src => src.MapFrom(x => x.Title))
            .ForMember(dest => dest.TaskGivers, src => src.MapFrom(x => x.TaskTakerRelations
                .Select(r => new WorkerPositionSummary
                {
                    Id = r.WorkerPosition.Id,
                    Title = r.WorkerPosition.Title
                }).ToList()))
            .ForMember(dest => dest.TaskTakers, src => src.MapFrom(x => x.TaskGiverRelations
                .Select(r => new WorkerPositionSummary
                {
                    Id = r.SubordinateWorkerPosition.Id,
                    Title = r.SubordinateWorkerPosition.Title
                }).ToList()));
    }
}