using AdessoTurkey.Application.DTOs;
using AdessoTurkey.Domain.Entities;
using AutoMapper;

namespace AdessoTurkey.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DrawRequestDto, Draw>()
                .ForMember(dest => dest.DrawDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Groups, opt => opt.Ignore());

            CreateMap<Draw, DrawResponseDto>()
                .ForMember(dest => dest.DrawerFullName,
                    opt => opt.MapFrom(src => $"{src.DrawerFirstName} {src.DrawerLastName}"))
                .ForMember(dest => dest.Groups,
                    opt => opt.MapFrom(src => src.Groups));

            CreateMap<DrawGroup, DrawGroupResponseDto>()
                .ForMember(dest => dest.Teams,
                    opt => opt.MapFrom(src => src.Teams));

            CreateMap<DrawTeam, DrawTeamResponseDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Team.Name));
        }
    }
}
