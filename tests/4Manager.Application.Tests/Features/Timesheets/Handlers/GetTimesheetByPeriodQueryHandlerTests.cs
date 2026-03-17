using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class GetTimesheetByPeriodQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly TimesheetTestFixture _fixture;

        public GetTimesheetByPeriodQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();
            _fixture = new TimesheetTestFixture();
        }

        [Fact]
        public async Task GetTimesheetsByPeriod()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockLogger = new Mock<ILogger<GetTimesheetByPeriodQueryHandler>>();

            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var userId = Guid.NewGuid();

            var fakeTimesheets = _fixture.GeneratesTimesheet(2).ToList();

            mockRepository.Setup(repo => repo.GetByDateRangeAsync(startDate, endDate, userId, CancellationToken.None))
                .ReturnsAsync(fakeTimesheets);

            var handler = new GetTimesheetByPeriodQueryHandler(mockRepository.Object, _mapper, mockLogger.Object);

            var result = await handler.Handle(new GetTimesheetsByPeriodQuery(startDate, endDate, userId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ReturnEmptyListWhenNoTimesheet() {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<GetTimesheetByPeriodQueryHandler>>();
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);

            mockRepository
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Timesheet>());

            mockMapper
                .Setup(m => m.Map<IEnumerable<TimesheetResponseDto>>(It.IsAny<IEnumerable<Project>>()))
                .Returns(new List<TimesheetResponseDto>());

            var handler = new GetTimesheetByPeriodQueryHandler(mockRepository.Object, mockMapper.Object, mockLogger.Object);

            var result = await handler.Handle(new GetTimesheetsByPeriodQuery(startDate, endDate, userId), CancellationToken.None);

            Assert.Empty(result);
        }
    }
}