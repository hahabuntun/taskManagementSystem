using AutoMapper;
using Vkr.API.Contracts.WorkersManagmentContracts;
using Vkr.Domain.DTO.Worker;

namespace Vkr.API.Profiles.WorkerProfiles;

public class WorkerManagmentProfile : Profile
{
    public WorkerManagmentProfile()
    {
        CreateMap<CreateManagerToEmployeeRequest, WorkersManagmentDTO>();

        CreateMap<WorkersManagmentDTO, ManagerToEmployeeResponse>();
    }
}