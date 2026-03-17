using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class TimesheetProfile : Profile
    {
        public TimesheetProfile() {
            CreateMap<Timesheet, TimesheetResponseDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.ProjectName : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                    src.Customer != null
                        ? src.Customer.Name
                        : string.Empty))
                .ForMember(dest => dest.ActivityColor, opt => opt.MapFrom(src => src.ActivityType.ActivityTypeColor))
                .ForMember(dest => dest.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityType.ActivityTypeName))
                .ForMember(dest => dest.BlockColor, opt => opt.MapFrom(src => src.Project.TitleColor));

            CreateMap<Timesheet, TimesheetReportResponseDto>()
                .ForMember(dest => dest.Duration,opt => opt.MapFrom(src => src.EndDate - src.StartDate))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.ProjectName))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Project.Customer.Name))
                .ForMember(dest => dest.BlockColor, opt => opt.MapFrom(src => src.Project.TitleColor))
                .ForMember(dest => dest.ActivityColor, opt => opt.MapFrom(src => src.ActivityType.ActivityTypeColor))
                .ForMember(dest => dest.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityType.ActivityTypeName));
        }
    }
}
