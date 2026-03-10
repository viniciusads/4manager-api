using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class UpdateTimesheetCommandHandler : IRequestHandler<UpdateTimesheetCommand, TimesheetResponseDto>
    {
        public readonly ITimesheetRepository _repository;
        public readonly IMapper _mapper;

        public UpdateTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TimesheetResponseDto> Handle(UpdateTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken);

            if (timesheet == null)
                throw new NotFoundException($"O timesheetId: {request.TimesheetId} não foi encontrado.");

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException("Você não tem permissão para atualizar este timesheet.");

            timesheet.StartDate = request.StartDate;
            timesheet.EndDate = request.EndDate;
            timesheet.Date = request.StartDate.Date;
            timesheet.Description = request.Description;

            await _repository.UpdateTimesheetAsync(timesheet, cancellationToken);

            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
