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
    public class UpdateTimesheetCommandHandler : IRequestHandler<UpdateTimesheetCommand, TimesheetResponseDto>
    {
        public readonly ITimesheetRepository _repository;
        public readonly IMapper _mapper;
        public readonly ILogger<UpdateTimesheetCommandHandler> _logger;

        public UpdateTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper, ILogger<UpdateTimesheetCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimesheetResponseDto> Handle(UpdateTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken);

            if (timesheet == null)
                throw new GuidFoundException($"O timesheetId: {request.TimesheetId} não foi encontrado.");

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedAction);

            timesheet.StartDate = request.StartDate;
            timesheet.EndDate = request.EndDate;
            timesheet.Date = request.StartDate.Date;
            timesheet.Description = request.Description;
            timesheet.ProjectId = request.ProjectId;
            timesheet.CustomerId = request.CustomerId;
            timesheet.ActivityTypeId = request.ActivityTypeId;

            await _repository.UpdateTimesheetAsync(timesheet, cancellationToken);

            _logger.LogInformation(Messages.Timesheet.Updated);

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
