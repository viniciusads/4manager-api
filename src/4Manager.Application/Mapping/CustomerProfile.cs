using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerResponseDto>();
        }
    }
}