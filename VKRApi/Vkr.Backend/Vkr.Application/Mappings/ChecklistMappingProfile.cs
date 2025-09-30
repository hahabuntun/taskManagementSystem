using AutoMapper;
using Vkr.Domain.DTO.Checklist;
using Vkr.Domain.Entities.CheckLists;

namespace Vkr.Application.Mappings;

public class ChecklistMappingProfile : Profile
{
    public ChecklistMappingProfile()
    {
        CreateMap<Checklist, ChecklistDto>();
        CreateMap<ChecklistItem, ChecklistItemDto>();
    }
}