using _4Tech._4Manager.Application.Features.TimesheetReports.Services;
using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.TimesheetReports.Handlers;
using _4Tech._4Manager.Application.Features.TimesheetReports.Queries;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.TimesheetReports.Handlers
{
    public class GetTimesheetReportByDataRangeQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly TimesheetTestFixture _fixture;
        private readonly Mock<ITimesheetReportCalculator> _calculator;

        public GetTimesheetReportByDataRangeQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();
            _fixture = new TimesheetTestFixture();
            _calculator = new Mock<ITimesheetReportCalculator>();
        }

        [Fact]
        public async Task GetTimesheetReportWhenAvaiable() {

            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<ITimesheetRepository>();

            var startDate = new DateTime(2026, 01, 01);
            var endDate = new DateTime(2026, 01, 03);
            var userId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(2);
            var fakeTimesheetResponseDto = new List<TimesheetReportResponseDto>()
            {
                new TimesheetReportResponseDto(),
                new TimesheetReportResponseDto()
            };

            fakeTimesheet[0].StartDate = new DateTime(2026, 01, 01);
            fakeTimesheet[0].EndDate = new DateTime(2026, 01, 01); 
            fakeTimesheet[0].UserId = userId;

            fakeTimesheet[1].StartDate = new DateTime(2026, 01, 02);
            fakeTimesheet[1].EndDate = new DateTime(2026, 01, 02);
            fakeTimesheet[1].UserId = userId;

            var expectedHours = new TimesheetReportDataGroupResponseDto()
            {
                TotalHours = "10:00:00"
            };
            
            mockRepository.Setup(repo => repo.GetByDateRangeAsync(startDate, endDate, userId, CancellationToken.None))
                .ReturnsAsync(fakeTimesheet);

            mockMapper.Setup(t => t.Map<List<TimesheetReportResponseDto>>(fakeTimesheet))
                .Returns(fakeTimesheetResponseDto);

            _calculator.Setup(c => c.Calculate(fakeTimesheetResponseDto))
                .Returns(expectedHours);

            var handler = new GetTimesheetReportByPeriodQueryHandler(mockRepository.Object, _calculator.Object, mockMapper.Object);

            var result = await handler.Handle(new GetTimesheetReportByDataRangeQuery(startDate, endDate, userId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.InRange(fakeTimesheet[0].StartDate, startDate, endDate);
            Assert.InRange(fakeTimesheet[1].StartDate, startDate, endDate);

            mockRepository.Verify(r => r.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<List<TimesheetReportResponseDto>>(It.IsAny<List<Timesheet>>()), Times.Once);
        }

        [Fact]
        public void CalculateReturnReportResults()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var timesheetDtos = new List<TimesheetReportResponseDto>
            {
                new TimesheetReportResponseDto
                {
                    StartDate = new DateTime(2025, 01, 01, 09, 00, 00),
                    EndDate = new DateTime(2025, 01, 01, 14, 00, 00),
                    ActivityTypeId = Guid.NewGuid(),
                    ActivityTypeName = "Desenvolvimento"
                },
                new TimesheetReportResponseDto
                {
                    StartDate = new DateTime(2025, 01, 02, 10, 00, 00),
                    EndDate = new DateTime(2025, 01, 02, 15, 00, 00),
                    ActivityTypeId = Guid.NewGuid(),
                    ActivityTypeName = "Reunião"
                }
            };

            var result = new TimesheetReportCalculator().Calculate(timesheetDtos);

            Assert.Equal("10:00:00", result.TotalHours);
            Assert.Equal("50", result.ActivityPercentages.First(a => a.ActivityTypeName == "Reunião").Percentage);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyReport_WhenNoTimesheetsFoundInPeriod()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var startDate = new DateTime(2026, 02, 01);
            var endDate = new DateTime(2026, 02, 28);
            var userId = Guid.NewGuid();


            var emptyTimesheets = new List<Timesheet>();
            var emptyDtos = new List<TimesheetReportResponseDto>();
            var expectedReport = new TimesheetReportDataGroupResponseDto();


            mockRepository
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyTimesheets);

            mockMapper
                .Setup(m => m.Map<List<TimesheetReportResponseDto>>(emptyTimesheets))
                .Returns(emptyDtos);

            _calculator
                .Setup(c => c.Calculate(emptyDtos))
                .Returns(expectedReport);

            var handler = new GetTimesheetReportByPeriodQueryHandler(mockRepository.Object, _calculator.Object, mockMapper.Object);


            var result = await handler.Handle( new GetTimesheetReportByDataRangeQuery(startDate, endDate, userId), CancellationToken.None);

            Assert.NotNull(result);
            
            mockRepository.Verify(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()), Times.Once);

            mockMapper.Verify(m => m.Map<List<TimesheetReportResponseDto>>(emptyTimesheets), Times.Once);

            _calculator.Verify(c => c.Calculate(emptyDtos), Times.Once);

        }


        
        [Fact]
        public async Task Handle_ReturnsCorrectDto_WhenTimesheetsExistInPeriod()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var startDate = new DateTime(2026, 03, 01);
            var endDate = new DateTime(2026, 03, 31);
            var userId = Guid.NewGuid();

            var fakeTimesheets = _fixture.GeneratesTimesheet(1);
            var fakeDtos = new List<TimesheetReportResponseDto> { new TimesheetReportResponseDto() };


            var expectedReport = new TimesheetReportDataGroupResponseDto
            {
                TotalHours = "05:00:00",
                Timesheets = fakeDtos,
                ActivityPercentages = new List<ActivityTypePercentageDto>(),
                ProjectPercentages = new List<ProjectPercentageDto>()
            };


            mockRepository
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheets);

            mockMapper
                .Setup(m => m.Map<List<TimesheetReportResponseDto>>(fakeTimesheets))
                .Returns(fakeDtos);

            _calculator
                .Setup(c => c.Calculate(fakeDtos))
                .Returns(expectedReport);


            var handler = new GetTimesheetReportByPeriodQueryHandler(mockRepository.Object, _calculator.Object, mockMapper.Object);

            
            var result = await handler.Handle( new GetTimesheetReportByDataRangeQuery(startDate, endDate, userId), CancellationToken.None);

            
                Assert.NotNull(result);
                Assert.Equal("05:00:00", result.TotalHours);
                Assert.Equal(fakeDtos, result.Timesheets);
                _calculator.Verify(c => c.Calculate(fakeDtos), Times.Once);

        }


        [Fact]
        public void Calculate_ReturnsZeroHoursAndEmptyLists_WhenTimesheetsListIsEmpty()
        {
            var emptyList = new List<TimesheetReportResponseDto>();

            var result = new TimesheetReportCalculator().Calculate(emptyList);


            Assert.Equal("00:00:00", result.TotalHours);
            Assert.Empty(result.Timesheets);
            Assert.Empty(result.ActivityPercentages);
            Assert.Empty(result.ProjectPercentages);

        }


        
        [Fact]
        public void Calculate_ReturnsHundredPercent_WhenSingleTimesheetExists()
        {
          
          var timesheetDtos = new List<TimesheetReportResponseDto>
          {
            new TimesheetReportResponseDto
                  {
                  StartDate        = new DateTime(2026, 04, 01, 08, 00, 00),
                  EndDate          = new DateTime(2026, 04, 01, 10, 00, 00),
                  ActivityTypeId   = Guid.NewGuid(),
                  ActivityTypeName = "Desenvolvimento",
                  ProjectId        = Guid.NewGuid(),
                  ProjectName      = "Projeto Alpha"
                  }
          };

            
            var result = new TimesheetReportCalculator().Calculate(timesheetDtos);

            
            Assert.Equal("02:00:00", result.TotalHours);

            Assert.Single(result.ActivityPercentages);
            Assert.Equal("100", result.ActivityPercentages.First().Percentage);

            Assert.Single(result.ProjectPercentages);
            Assert.Equal("100", result.ProjectPercentages.First().Percentage);
        }



        
        [Fact]    
        public void Calculate_ThrowsException_WhenTimesheetHasNullEndDate()
        {
            var timesheetDtos = new List<TimesheetReportResponseDto>
            {
                new TimesheetReportResponseDto 
                {
                    StartDate        = new DateTime(2026, 05, 01, 09, 00, 00),
                    EndDate          = null,
                    ActivityTypeId   = Guid.NewGuid(),
                    ActivityTypeName = "Desenvolvimento"
                }
            };

            Assert.Throws<InvalidOperationException>( 
                () => new TimesheetReportCalculator().Calculate(timesheetDtos)
               );
        }


        [Fact]
        public void Calculate_ReturnsCorrectProjectPercentages_WhenMultipleProjectsExist()
        {
            var projectAlphaId = Guid.NewGuid();
            var projectBetaId = Guid.NewGuid();
            var activityTypeId = Guid.NewGuid();

            var timesheetDtos = new List<TimesheetReportResponseDto>
      {
          new TimesheetReportResponseDto
          {
              StartDate        = new DateTime(2026, 06, 01, 08, 00, 00),
              EndDate          = new DateTime(2026, 06, 01, 10, 00, 00),
              ActivityTypeId   = activityTypeId,
              ActivityTypeName = "Desenvolvimento",
              ProjectId        = projectAlphaId,
              ProjectName      = "Projeto Alpha"
          },
          new TimesheetReportResponseDto
          {
              StartDate        = new DateTime(2026, 06, 02, 08, 00, 00),
              EndDate          = new DateTime(2026, 06, 02, 10, 00, 00),
              ActivityTypeId   = activityTypeId,
              ActivityTypeName = "Desenvolvimento",
              ProjectId        = projectAlphaId,
              ProjectName      = "Projeto Alpha"
          },
          new TimesheetReportResponseDto
          {
              StartDate        = new DateTime(2026, 06, 03, 09, 00, 00),
              EndDate          = new DateTime(2026, 06, 03, 10, 00, 00),
              ActivityTypeId   = activityTypeId,
              ActivityTypeName = "Desenvolvimento",
              ProjectId        = projectBetaId,
              ProjectName      = "Projeto Beta"
          }
      };

            
            var result = new TimesheetReportCalculator().Calculate(timesheetDtos);

            
            Assert.Equal(2, result.ProjectPercentages.Count);

            var alphaPercentage = result.ProjectPercentages.First(p => p.ProjectName == "Projeto Alpha");
            var betaPercentage = result.ProjectPercentages.First(p => p.ProjectName == "Projeto Beta");

            Assert.Equal("80", alphaPercentage.Percentage);
            Assert.Equal("20", betaPercentage.Percentage);
        }
    }
}
