using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Commands
{
    public class CreateTicketNoteCommand : IRequest<Guid>
    {
        public Guid TicketDetailsId { get; set; }
        public string NoteText { get; set; } = string.Empty;
    }
}
