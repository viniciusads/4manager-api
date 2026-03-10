using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class DeleteTimesheetCommandHandler : IRequestHandler<DeleteTimesheetCommand, TimesheetResponseDto>
    {
        private readonly ITimesheetRepository _repository;
        private readonly IMapper _mapper;

        public DeleteTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TimesheetResponseDto> Handle(DeleteTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken);

            if (timesheet == null)
                throw new NotFoundException($"O timesheetId: {request.TimesheetId} não foi encontrado.");

            if (timesheet.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException("Você não tem permissão para deletar este timesheet.");

            await _repository.DeleteTimesheetAsync(request.TimesheetId, cancellationToken);
            return _mapper.Map<TimesheetResponseDto>(timesheet);
        }
    }
}
