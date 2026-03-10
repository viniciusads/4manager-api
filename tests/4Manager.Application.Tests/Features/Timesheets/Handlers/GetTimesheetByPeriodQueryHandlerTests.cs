using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class GetTimesheetByPeriodQueryHandlerTests
    {
        private readonly IMapper _mapper;


        public GetTimesheetByPeriodQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnListOfTimesheetByDateRangeResponseDto()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var userId = Guid.NewGuid();

            var fakeTimesheets = new List<Timesheet>
            {
                new Timesheet
                {
                    TimesheetId = Guid.NewGuid(),
                    StartDate = startDate,
                    EndDate = endDate
                },

                new Timesheet
                {
                    TimesheetId = Guid.NewGuid(),
                    StartDate = startDate,
                    EndDate = endDate
                }
            };

            mockRepository.Setup(repo => repo.GetByDateRangeAsync(startDate, endDate, userId, CancellationToken.None))
                .ReturnsAsync(fakeTimesheets);

            var handler = new GetTimesheetByPeriodQueryHandler(mockRepository.Object, _mapper);

            var result = await handler.Handle(new GetTimesheetsByPeriodQuery(startDate, endDate, userId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.StartDate == startDate);
            Assert.Contains(result, c => c.EndDate == endDate);
        }
    }
}