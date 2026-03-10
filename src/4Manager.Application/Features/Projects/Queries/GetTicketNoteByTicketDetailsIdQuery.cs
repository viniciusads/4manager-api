using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public record GetTicketNoteByTicketDetailsIdQuery(Guid NoteId) : IRequest<TicketDetailsResponseDto>
    {
        public Guid NoteId { get; set; } = NoteId;
    }
}
