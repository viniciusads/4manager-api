using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class StartTimerTimesheetCommandHandlerTests
    {
        private readonly Mock<ITimesheetRepository> _repositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<StartTimerTimesheetCommandHandler>> _loggerMock = new();
        private readonly Mock<IActivityTypeRepository> _activityTypeRepositoryMock = new();
        private readonly StartTimerTimesheetCommandHandler _handler;
        private readonly TimesheetTestFixture _fixture;

        public StartTimerTimesheetCommandHandlerTests()
        {
            _handler = new StartTimerTimesheetCommandHandler(_repositoryMock.Object, _mapperMock.Object, _activityTypeRepositoryMock.Object, _loggerMock.Object);
            _fixture = new TimesheetTestFixture();
        }

        [Fact]
        public async Task StartTimesheetTimer()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var date = new DateTime();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.Description = "Descrição teste";
            fakeTimesheet.UserId = Guid.NewGuid();
            fakeTimesheet.ProjectId = projectId;

            var command = new StartTimerTimesheetCommand(fakeTimesheet.StartDate, fakeTimesheet.Description, userId, projectId, fakeTimesheet.ActivityTypeId);

            var timesheetDto = new TimesheetResponseDto
            {
                TimesheetId = Guid.NewGuid(),
                StartDate = fakeTimesheet.StartDate,
                Description = fakeTimesheet.Description,
                Date = fakeTimesheet.Date,
                UserId = fakeTimesheet.UserId
            };

            var fakeActivity = new ActivityType
            {
                ActivityTypeId = Guid.NewGuid(),
                IsActive = true
            };

            _activityTypeRepositoryMock
                .Setup(repo => repo.GetActivityTypeById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivity);

            _repositoryMock.Setup(r => r.StartTimerTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()))
                .Returns(timesheetDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(timesheetDto.TimesheetId, result.TimesheetId);
            Assert.Equal(timesheetDto.Description, result.Description);

            _repositoryMock.Verify(r => r.StartTimerTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()), Times.Once);
        }

        [Fact]
        public async Task Should_Use_First_Active_ActivityType_When_Default_Is_Inactive()
        {
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;

            var defaultActivity = new ActivityType
            {
                ActivityTypeId = Guid.NewGuid(),
                IsActive = false
            };

            var fallbackActivity = new ActivityType
            {
                ActivityTypeId = Guid.NewGuid(),
                IsActive = true
            };

            _activityTypeRepositoryMock
                .Setup(x => x.GetActivityTypeById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(defaultActivity);

            _activityTypeRepositoryMock
                .Setup(x => x.GetFirstActiveActivityTypeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(fallbackActivity);

            _repositoryMock
                .Setup(x => x.StartTimerTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(x => x.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()))
                .Returns(new TimesheetResponseDto());

            var command = new StartTimerTimesheetCommand(startDate, "desc", userId, projectId, Guid.NewGuid());

            await _handler.Handle(command, CancellationToken.None);

            _activityTypeRepositoryMock.Verify(x =>
                x.GetFirstActiveActivityTypeAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Use_First_Active_ActivityType_When_Default_Is_Null()
        {
            var fallbackActivity = new ActivityType
            {
                ActivityTypeId = Guid.NewGuid(),
                IsActive = true
            };

            _activityTypeRepositoryMock
                .Setup(x => x.GetActivityTypeById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ActivityType?)null);

            _activityTypeRepositoryMock
                .Setup(x => x.GetFirstActiveActivityTypeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(fallbackActivity);

            _repositoryMock
                .Setup(x => x.StartTimerTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(x => x.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()))
                .Returns(new TimesheetResponseDto());

            var command = new StartTimerTimesheetCommand(DateTime.UtcNow, null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            await _handler.Handle(command, CancellationToken.None);

            _activityTypeRepositoryMock.Verify(x =>
                x.GetFirstActiveActivityTypeAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
