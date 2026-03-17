using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class StopTimerTimesheetCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly TimesheetTestFixture _fixture;
        private readonly Mock<ILogger<StopTimerTimesheetCommandHandler>> _loggerMock = new();

        public StopTimerTimesheetCommandHandlerTests() {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();

            _fixture = new TimesheetTestFixture();
        }

        [Fact]
        public async Task StopTimesheetTimer()
        {
            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var endDate = new DateTime(2025, 01, 01);
            var description = "Descrição - patch endDate";

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.UserId = userId;
            fakeTimesheet.TimesheetId = timesheetId;
            fakeTimesheet.ProjectId = projectId;
            fakeTimesheet.Description = "Descrição - patch endDate";


            var mockRepo = new Mock<ITimesheetRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);
            mockRepo.Setup(r => r.StopTimerTimesheetAsync(timesheetId, endDate, description,projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);

            var command = new StopTimerTimesheetCommand(endDate, timesheetId, description, userId, projectId);
            var handler = new StopTimerTimesheetCommandHandler(mockRepo.Object, _mapper, _loggerMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(timesheetId, result.TimesheetId);
            Assert.Equal(description, result.Description);

            mockRepo.Verify(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.StopTimerTimesheetAsync(timesheetId, endDate, description, projectId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task StopTimerFailIfTimesheetNotFound()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var endDate = new DateTime(2025, 01, 01);
            var description = "Descrição - patch endDate";

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.UserId = userId;
            fakeTimesheet.ProjectId = projectId;
            fakeTimesheet.TimesheetId = timesheetId;

            mockRepository
                .Setup(repo => repo.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new GuidFoundException($"Timesheet com id {timesheetId} não encontrado."));

            var command = new StopTimerTimesheetCommand(endDate, timesheetId, description, userId, projectId);
            var handler = new StopTimerTimesheetCommandHandler(mockRepository.Object, mockMapper.Object, _loggerMock.Object);

            var exceptionMessage = $"Timesheet com id {timesheetId} não encontrado.";
            var exception = await Assert.ThrowsAsync<GuidFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal(exceptionMessage, exception.Message);

            mockRepository.Verify(r => r.StopTimerTimesheetAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task StopTimerUnauthorizedIfTokenInvalid()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var endDate = new DateTime(2025, 01, 01);

            var differentUserId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.TimesheetId = timesheetId;
            fakeTimesheet.ProjectId = projectId;

            var command = new StopTimerTimesheetCommand(endDate, timesheetId, "Timer Teste", differentUserId, projectId);

            mockRepository.Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(fakeTimesheet);

            var handler = new StopTimerTimesheetCommandHandler(mockRepository.Object, mockMapper.Object, _loggerMock.Object);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal(Messages.Auth.UnauthorizedAction, exception.Message);

            mockRepository.Verify(r => r.StopTimerTimesheetAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
