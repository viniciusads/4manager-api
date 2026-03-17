using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class GetTimesheetByIdQueryHandler : IRequestHandler<GetTimesheetByIdQuery, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTimesheetByIdQueryHandler> _logger;

        public GetTimesheetByIdQueryHandler(ITimesheetRepository timesheetRepository, IMapper mapper, ILogger<GetTimesheetByIdQueryHandler> logger)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimesheetResponseDto> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.GetByIdAsync(request.TimesheetId, cancellationToken);

            _logger.LogInformation("O id do timesheet retornado foi {timesheetId}", request.TimesheetId);

            if (timesheet == null)
                throw new TimesheetException();

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedView);

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
