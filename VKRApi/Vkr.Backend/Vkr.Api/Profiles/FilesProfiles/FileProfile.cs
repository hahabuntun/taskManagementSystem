using AutoMapper;
using Vkr.API.Contracts.FilesContracts;
using File = Vkr.Domain.Entities.Files.File;


namespace Vkr.API.Profiles.FilesProfiles
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<File, FileResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.FileSize))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        }
    }
}
