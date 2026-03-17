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
    public class UpdateTimesheetQueryHandlerTests
    {

        private readonly IMapper _mapper;
        private readonly TimesheetTestFixture _fixture;
        private readonly Mock<ILogger<UpdateTimesheetCommandHandler>> _loggerMock = new();

        public UpdateTimesheetQueryHandlerTests() {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();

            _fixture = new TimesheetTestFixture();
        }

        [Fact]
        public async Task UpdateTimesheet()
        {
            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var projectId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.UserId = userId;
            fakeTimesheet.TimesheetId = timesheetId;
            fakeTimesheet.Description = "Descrição teste";

            var mockRepo = new Mock<ITimesheetRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);

            mockRepo.Setup(r => r.UpdateTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateTimesheetCommand(timesheetId, startDate, endDate, fakeTimesheet.Description, userId, fakeTimesheet.ActivityTypeId, projectId, customerId);
            var handler = new UpdateTimesheetCommandHandler(mockRepo.Object, _mapper, _loggerMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(timesheetId, result.TimesheetId);
            Assert.Equal("Descrição teste", result.Description);

            mockRepo.Verify(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.UpdateTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFailIfTimesheetNotFound()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var projectId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];

            mockRepository.Setup(repo => repo.GetByIdAsync(fakeTimesheet.TimesheetId, CancellationToken.None))
                .ThrowsAsync(new GuidFoundException($"Timesheet com id {fakeTimesheet.TimesheetId} não encontrado."));

            var query = new UpdateTimesheetCommand(fakeTimesheet.TimesheetId, startDate, endDate, fakeTimesheet.Description, fakeTimesheet.UserId, fakeTimesheet.ActivityTypeId, projectId, customerId);
            var handler = new UpdateTimesheetCommandHandler(mockRepository.Object, _mapper, _loggerMock.Object);

            var exceptionMessage = $"Timesheet com id {fakeTimesheet.TimesheetId} não encontrado.";
            var exception = await Assert.ThrowsAsync<GuidFoundException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal(exceptionMessage, exception.Message);

            mockRepository.Verify(r => r.UpdateTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUnauthorizedIfTokenInvalid()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var customerId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var differentUserId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.UserId = userId;
            fakeTimesheet.TimesheetId = timesheetId;

            var command = new UpdateTimesheetCommand(timesheetId, startDate, endDate, fakeTimesheet.Description, differentUserId, fakeTimesheet.ActivityTypeId, projectId, customerId);

            mockRepository.Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(fakeTimesheet);

            var handler = new UpdateTimesheetCommandHandler(mockRepository.Object, mockMapper.Object, _loggerMock.Object);

            var exceptionMessage = Messages.Auth.UnauthorizedAction;
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal(exceptionMessage, exception.Message);

            mockRepository.Verify(r => r.UpdateTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
