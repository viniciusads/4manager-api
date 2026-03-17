using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public class GetTicketDetailsByTicketIdQuery(Guid ticketId) : IRequest<TicketDetailsResponseDto>
    {
        public Guid ticketId { get; set; } = ticketId;
    }
}
