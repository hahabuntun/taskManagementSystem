using AutoMapper;
using Vkr.Domain.DTO.Board;
using Vkr.Domain.Entities.Board;

namespace Vkr.Application.Mappings;

public class BoardMappingProfile : Profile
{
    public BoardMappingProfile()
    {
        CreateMap<Boards, BoardDto>();
    }
}