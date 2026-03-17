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
    public class CreateManualTimesheetCommandHandlerTests
    {
        private readonly Mock<ITimesheetRepository> _repositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CreateManualTimesheetCommandHandler>> _loggerMock = new();
        private readonly CreateManualTimesheetCommandHandler _handler;
        private readonly TimesheetTestFixture _fixture;

        public CreateManualTimesheetCommandHandlerTests()
        {
            _handler = new CreateManualTimesheetCommandHandler(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            _fixture = new TimesheetTestFixture();
        }

        [Fact]
        public async Task CreateManualTimesheet()
        {
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01, 09, 00, 00);
            var endDate = new DateTime(2025, 01, 01, 18, 00, 00);
            var description = "Descrição teste";
            var activityTypeId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var command = new CreateManualTimesheetCommand(startDate, endDate, description, userId, activityTypeId, projectId, customerId);

            var fakeTimesheetDto = new TimesheetResponseDto
            {
                TimesheetId = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                Description = description,
                UserId = userId
            };

            _repositoryMock.Setup(r => r.CreateManualTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()))
                .Returns(fakeTimesheetDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(fakeTimesheetDto.TimesheetId, result.TimesheetId);
            Assert.Equal(description, result.Description);
            Assert.Equal(userId, result.UserId);

            _repositoryMock.Verify(r => r.CreateManualTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()), Times.Once);
        }

        [Fact]
        public async Task CreateManualTimesheet_Should_Create_Entity_With_Correct_Values()
        {
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01, 09, 00, 00);
            var endDate = new DateTime(2025, 01, 01, 18, 00, 00);
            var description = "Descrição teste";
            var activityTypeId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var command = new CreateManualTimesheetCommand(
                startDate,
                endDate,
                description,
                userId,
                activityTypeId,
                projectId,
                customerId);

            Timesheet? capturedTimesheet = null;

            _repositoryMock
                .Setup(r => r.CreateManualTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Callback<Timesheet, CancellationToken>((t, _) => capturedTimesheet = t)
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()))
                .Returns(new TimesheetResponseDto());

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedTimesheet);
            Assert.Equal(startDate, capturedTimesheet.StartDate);
            Assert.Equal(startDate.Date, capturedTimesheet.Date);
            Assert.Equal(endDate, capturedTimesheet.EndDate);
            Assert.Equal(description, capturedTimesheet.Description);
            Assert.Equal(userId, capturedTimesheet.UserId);
            Assert.Equal(projectId, capturedTimesheet.ProjectId);
            Assert.Equal(customerId, capturedTimesheet.CustomerId);
            Assert.Equal(activityTypeId, capturedTimesheet.ActivityTypeId);

            _repositoryMock.Verify(r =>
                r.CreateManualTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateManualTimesheet_Should_Set_Empty_String_When_Description_Is_Null()
        {
            var command = new CreateManualTimesheetCommand(
                DateTime.Now,
                DateTime.Now.AddHours(1),
                string.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            Timesheet? capturedTimesheet = null;

            _repositoryMock
                .Setup(r => r.CreateManualTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Callback<Timesheet, CancellationToken>((t, _) => capturedTimesheet = t)
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()))
                .Returns(new TimesheetResponseDto());

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedTimesheet);
            Assert.Equal(string.Empty, capturedTimesheet.Description);
        }
    }
}
