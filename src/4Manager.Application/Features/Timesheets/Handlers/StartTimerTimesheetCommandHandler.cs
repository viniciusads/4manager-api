using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using MediatR;
namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class StartTimerTimesheetCommandHandler : IRequestHandler<StartTimerTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public StartTimerTimesheetCommandHandler(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<TimesheetResponseDto> Handle(StartTimerTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = new Timesheet
            {
                StartDate = request.StartDate,
                Date = request.StartDate.Date,
                EndDate = null,
                Description = request.Description ?? string.Empty,
                UserId = request.AuthenticatedUserId
            };

            await _timesheetRepository.StartTimerTimesheetAsync(timesheet, cancellationToken);
            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
