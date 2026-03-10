using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class StartTimerTimesheetCommandHandlerTests
    {
        private readonly Mock<ITimesheetRepository> _repositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly StartTimerTimesheetCommandHandler _handler;

        public StartTimerTimesheetCommandHandlerTests()
        {
            _handler = new StartTimerTimesheetCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleStartTimerWhenValid()
        {

            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01);
            var date = new DateTime();
            var description = "Descrição teste";

            var command = new StartTimerTimesheetCommand(startDate, description, userId);
            var fakeTimesheet = new Timesheet
            {
                StartDate = startDate,
                Description = description,
                Date = date,
                UserId = userId
            };
            var timesheetDto = new TimesheetResponseDto
            {
                TimesheetId = Guid.NewGuid(),
                StartDate = fakeTimesheet.StartDate,
                Description = fakeTimesheet.Description,
                Date = fakeTimesheet.Date,
                UserId = fakeTimesheet.UserId
            };

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
    }
}
