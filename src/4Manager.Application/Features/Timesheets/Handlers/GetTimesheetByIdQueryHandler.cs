using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class GetTimesheetByIdQueryHandler : IRequestHandler<GetTimesheetByIdQuery, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public GetTimesheetByIdQueryHandler(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<TimesheetResponseDto> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.GetByIdAsync(request.TimesheetId, cancellationToken);

            if (timesheet == null)
                throw new NotFoundException($"O timesheetId: {request.TimesheetId} não foi encontrado.");

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException("Você não tem permissão para visualizar este timesheet.");

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
