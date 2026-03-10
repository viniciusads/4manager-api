using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class CreateManualTimesheetCommandHandler : IRequestHandler<CreateManualTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public CreateManualTimesheetCommandHandler(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<TimesheetResponseDto> Handle(CreateManualTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = new Timesheet
            {
                StartDate = request.StartDate,
                Date = request.StartDate.Date,
                EndDate = request.EndDate,
                Description = request.Description ?? string.Empty,
                UserId = request.AuthenticatedUserId
            };

            await _timesheetRepository.CreateManualTimesheetAsync(timesheet, cancellationToken);
            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
