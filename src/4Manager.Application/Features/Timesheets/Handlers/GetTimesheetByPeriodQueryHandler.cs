using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Handlers
{
    public class GetTimesheetByPeriodQueryHandler : IRequestHandler<GetTimesheetsByPeriodQuery, IEnumerable<TimesheetResponseDto>>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public GetTimesheetByPeriodQueryHandler(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TimesheetResponseDto>> Handle(GetTimesheetsByPeriodQuery request, CancellationToken cancellationToken)
        {
            var timesheets = await _timesheetRepository.GetByDateRangeAsync(
                request.StartDate, 
                request.EndDate, 
                request.AuthenticatedUserId,
                cancellationToken
            );

            return _mapper.Map<IEnumerable<TimesheetResponseDto>>(timesheets);
        }
    }
}
