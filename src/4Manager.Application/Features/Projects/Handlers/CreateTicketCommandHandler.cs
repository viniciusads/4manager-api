using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using MediatR;
namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
    {
        private readonly ITicketRepository _TicketRepository;
        private readonly IProjectRepository _ProjectRepository;

        public CreateTicketCommandHandler(ITicketRepository ticketRepository, IProjectRepository projectRepository)
        {
            _TicketRepository = ticketRepository;
            _ProjectRepository = projectRepository;
        }

        public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {

            int nextTicketNumber = 0;
            nextTicketNumber = await _TicketRepository.GetNextTicketNumberAsync(cancellationToken);

            var newTicket = new Ticket
            {
                TicketId = Guid.NewGuid(),

                TicketNumber = nextTicketNumber,

                ProjectId = request.ProjectId,

                Applicant = request.Applicant,
                Sector = request.Sector,
                Description = request.Description,
                AffectedSystem = request.AffectedSystem,

                InternalCall = Guid.NewGuid(),
                TicketResponsible = request.TicketResponsible,
                ResponsibleArea = request.ResponsibleArea,
                Priority = request.Priority,

                OpeningDate = request.OpeningDate.ToUniversalTime(),
                DeadlineDate = request.DeadlineDate.ToUniversalTime(),

                TicketStatus = TicketStatusEnum.Aberto
            };

            var project = await _ProjectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
                throw new ProjectException();

            await _TicketRepository.CreateTicketAsync(newTicket, cancellationToken);

            return newTicket.TicketId;
        }
    }
}
