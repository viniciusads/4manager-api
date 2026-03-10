using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetTicketsByProjectIdQueryHandler : IRequestHandler<GetTicketsByProjectIdQuery, IEnumerable<TicketResponseDto>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketsByProjectIdQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketResponseDto>> Handle(GetTicketsByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetByProjectIdAsync(
                request.ProjectId,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            if (tickets == null || !tickets.Any())
                throw new NotFoundException("Não existe um ticket com esse id.");

            return _mapper.Map<IEnumerable<TicketResponseDto>>(tickets);
        }
    }
}
