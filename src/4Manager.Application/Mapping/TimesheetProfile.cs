using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class TimesheetProfile : Profile
    {
        public TimesheetProfile() {
            CreateMap<Timesheet, TimesheetResponseDto>();
        }
    }
}
