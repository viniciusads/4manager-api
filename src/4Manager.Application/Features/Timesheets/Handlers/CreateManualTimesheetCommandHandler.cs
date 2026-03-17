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
    public class CreateManualTimesheetCommandHandler : IRequestHandler<CreateManualTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateManualTimesheetCommandHandler> _logger;

        public CreateManualTimesheetCommandHandler(ITimesheetRepository timesheetRepository, IMapper mapper, ILogger<CreateManualTimesheetCommandHandler> logger)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimesheetResponseDto> Handle(CreateManualTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = new Timesheet
            {
                StartDate = request.StartDate,
                Date = request.StartDate.Date,
                EndDate = request.EndDate,
                Description = request.Description ?? string.Empty,
                ProjectId = request.ProjectId,
                CustomerId = request.CustomerId,
                UserId = request.AuthenticatedUserId,
                ActivityTypeId = request.ActivityTypeId
            };

            await _timesheetRepository.CreateManualTimesheetAsync(timesheet, cancellationToken);   
            
            _logger.LogInformation(Messages.Timesheet.Created);

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
