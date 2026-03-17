using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetTicketDetailsByTicketIdQueryHandler : IRequestHandler<GetTicketDetailsByTicketIdQuery, TicketDetailsResponseDto?>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketDetailsByTicketIdQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<TicketDetailsResponseDto?> Handle(GetTicketDetailsByTicketIdQuery request, CancellationToken cancellationToken)
        {
            var TicketDetails = await _ticketRepository.GetByTicketId(request.ticketId, cancellationToken);

            if (TicketDetails is null)
                throw new GuidException();

            return _mapper.Map<TicketDetailsResponseDto>(TicketDetails);
        }
    }
}
