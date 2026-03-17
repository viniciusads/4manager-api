using _4Tech._4Manager.Application.Features.ActivityTypes.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class ActivityTypeProfile : Profile
    {
        public ActivityTypeProfile() {
            CreateMap<ActivityType, ActivityTypeResponseDto>();
        }
    }
}
