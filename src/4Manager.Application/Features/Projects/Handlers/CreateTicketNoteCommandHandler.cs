using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Exceptions;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class CreateTicketNoteCommandHandler : IRequestHandler<CreateTicketNoteCommand, Guid>
    {
        private readonly ITicketRepository _repository;

        public CreateTicketNoteCommandHandler(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateTicketNoteCommand request, CancellationToken cancellationToken)
        {
            var newNote = new Note
            {
                NoteId = Guid.NewGuid(),
                TicketDetailsId = request.TicketDetailsId,
                NoteText = request.NoteText
            };

            var ticketDetailsIdExists = await _repository.TicketDetailsExistsAsync(request.TicketDetailsId, cancellationToken);

            if (!ticketDetailsIdExists)
                throw new NotFoundException("O TicketDetailsId informado não existe.");

            await _repository.CreateTicketNoteAsync(newNote, cancellationToken);

            return newNote.NoteId;
        }
    }
}

