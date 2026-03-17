using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class GetTimesheetByPeriodQueryHandler : IRequestHandler<GetTimesheetsByPeriodQuery, IEnumerable<TimesheetResponseDto>>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTimesheetByPeriodQueryHandler> _logger;

        public GetTimesheetByPeriodQueryHandler(ITimesheetRepository timesheetRepository, IMapper mapper, ILogger<GetTimesheetByPeriodQueryHandler> logger)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TimesheetResponseDto>> Handle(GetTimesheetsByPeriodQuery request, CancellationToken cancellationToken)
        {
            var timesheets = await _timesheetRepository.GetByDateRangeAsync(
                request.StartDate, 
                request.EndDate, 
                request.AuthenticatedUserId,
                cancellationToken
            );

            _logger.LogInformation(Messages.Timesheet.Returned);

            return _mapper.Map<IEnumerable<TimesheetResponseDto>>(timesheets);
        }
    }
}
