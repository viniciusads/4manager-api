using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;

namespace _4Tech._4Manager.Application.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketResponseDto>();
            CreateMap<TicketAttachment, TicketAttachmentResposeDto>();
            CreateMap<Ticket, TicketDetailsResponseDto>()
                .ForMember(dest => dest.TicketId, opt => opt.MapFrom(src => src.TicketId))

                .ForMember(dest => dest.TicketDetailsId, opt => opt.MapFrom(src => src.TicketDetails.TicketDetailsId))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.TicketDetails.Note))
                .ForMember(dest => dest.MessageHistory, opt => opt.MapFrom(src => src.TicketDetails.MessageHistory));
        }
    }
}
