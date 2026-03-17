using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;
namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public record GetTicketNoteByTicketDetailsIdQuery(Guid NoteId) : IRequest<TicketDetailsResponseDto>
    {
        public Guid NoteId { get; set; } = NoteId;
    }
}
