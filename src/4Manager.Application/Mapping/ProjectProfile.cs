using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.CustomerId ?? Guid.Empty));
        }
    }
}
