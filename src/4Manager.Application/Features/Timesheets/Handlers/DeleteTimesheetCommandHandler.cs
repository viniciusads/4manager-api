using _4Tech._4Manager.Application.Common.Exceptions;
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
    public class DeleteTimesheetCommandHandler : IRequestHandler<DeleteTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTimesheetCommandHandler> _logger;

        public DeleteTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper, ILogger<DeleteTimesheetCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimesheetResponseDto> Handle(DeleteTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken);

            if (timesheet == null)
                throw new TimesheetException();

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedAction);

            await _repository.DeleteTimesheetAsync(request.TimesheetId, cancellationToken);

            _logger.LogInformation(Messages.Timesheet.Deleted);

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
