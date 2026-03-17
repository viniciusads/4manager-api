using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class StartTimerTimesheetCommandHandler : IRequestHandler<StartTimerTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StartTimerTimesheetCommandHandler> _logger;

        public StartTimerTimesheetCommandHandler(
                ITimesheetRepository timesheetRepository, 
                IMapper mapper, 
                IActivityTypeRepository activityTypeRepository, 
                ILogger<StartTimerTimesheetCommandHandler> logger
            )
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _activityTypeRepository = activityTypeRepository;
            _logger = logger;
        }

        public async Task<TimesheetResponseDto> Handle(StartTimerTimesheetCommand request, CancellationToken cancellationToken)
        {
            
            var returnActivityTypeIdIfActive = await GetAnActiveActivityType(cancellationToken);

            var timesheet = new Timesheet
            {
                StartDate = request.StartDate,
                Date = request.StartDate.Date,
                EndDate = null,
                ProjectId = request.ProjectId,
                Description = request.Description ?? string.Empty,
                UserId = request.AuthenticatedUserId,
                ActivityTypeId = returnActivityTypeIdIfActive
            };

            await _timesheetRepository.StartTimerTimesheetAsync(timesheet, cancellationToken);

            _logger.LogInformation(Messages.Timesheet.TimerInit);

            _logger.LogInformation("ActivityType ativo encontrado: {activityTypeId}", timesheet.ActivityTypeId);

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }

        private async Task<Guid> GetAnActiveActivityType(CancellationToken cancellationToken)
        {
            var defaultActivityTypeId = Guid.Parse("edf8a990-e5b6-4749-8c25-2af879049699");
            var activityTypeId = await _activityTypeRepository.GetActivityTypeById(defaultActivityTypeId, cancellationToken);
            if (activityTypeId == null || !activityTypeId.IsActive)
            {
                activityTypeId = await _activityTypeRepository.GetFirstActiveActivityTypeAsync(cancellationToken);
            }
            return activityTypeId.ActivityTypeId;
        }
    }
}
