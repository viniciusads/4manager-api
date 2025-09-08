using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
   
}

