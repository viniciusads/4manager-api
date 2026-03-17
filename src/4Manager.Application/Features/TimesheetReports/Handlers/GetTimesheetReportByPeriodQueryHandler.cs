using _4Tech._4Manager.Application.Features.TimesheetReports.Services;
using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.TimesheetReports.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Handlers
{
    public class GetTimesheetReportByPeriodQueryHandler : IRequestHandler<GetTimesheetReportByDataRangeQuery, TimesheetReportDataGroupResponseDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly ITimesheetReportCalculator _timesheetCalculator;
        private readonly IMapper _mapper;

        public GetTimesheetReportByPeriodQueryHandler(ITimesheetRepository timesheetRepository, ITimesheetReportCalculator calculator, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _timesheetCalculator = calculator;
            _mapper = mapper;
        }

        public async Task<TimesheetReportDataGroupResponseDto> Handle(GetTimesheetReportByDataRangeQuery request, CancellationToken cancellationToken)
        {
            var timesheets = await _timesheetRepository.GetByDateRangeAsync(
                request.StartDate,
                request.EndDate,
                request.AuthenticatedUserId,
                cancellationToken
            );

            var timesheetDtos = _mapper.Map<List<TimesheetReportResponseDto>>(timesheets);

            var report = _timesheetCalculator.Calculate(timesheetDtos);

            return report;
        }
    }
}
