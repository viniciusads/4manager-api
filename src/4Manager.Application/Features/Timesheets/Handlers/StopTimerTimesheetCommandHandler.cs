using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class StopTimerTimesheetCommandHandler : IRequestHandler<StopTimerTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StopTimerTimesheetCommandHandler> _logger;
        
        public StopTimerTimesheetCommandHandler(ITimesheetRepository timesheetRepository, IMapper mapper, ILogger<StopTimerTimesheetCommandHandler> logger)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimesheetResponseDto> Handle(StopTimerTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.GetByIdAsync(request.TimesheetId, cancellationToken);

            if (timesheet == null)
                throw new GuidFoundException($"O timesheetId: {request.TimesheetId} não foi encontrado.");

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedAction);

            var updatedTimesheet = await _timesheetRepository.StopTimerTimesheetAsync(request.TimesheetId, request.EndDate, request.Description, request.ProjectId, cancellationToken);
            return _mapper.Map<TimesheetResponseDto>(updatedTimesheet);
        }
    }
}
