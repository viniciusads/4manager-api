using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class DeleteTimesheetCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapper = new();
        private readonly ILogger<DeleteTimesheetCommandHandler> _logger;
        private readonly Mock<ITimesheetRepository> _repositoryMock = new();
        private readonly TimesheetTestFixture _fixture;
        private DeleteTimesheetCommandHandler CreateHandler()
           => new DeleteTimesheetCommandHandler(
               _repositoryMock.Object,
               _mapper.Object,
               _logger);

        public DeleteTimesheetCommandHandlerTests() {
            _logger = new Mock<ILogger<DeleteTimesheetCommandHandler>>().Object;

            _fixture = new TimesheetTestFixture();
        }

             private static Timesheet CreateFakeTimesheet(Guid userId)
        {
            return new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                UserId = userId,
                Description = "Teste"
            };
        }
        

        [Fact]
        public async Task DeleteTimesheetByRequiredId()
        {
            var userId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];
            fakeTimesheet.UserId = userId;

            var mockRepo = new Mock<ITimesheetRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);

            mockRepo.Setup(r => r.DeleteTimesheetAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapper
                .Setup(m => m.Map<TimesheetResponseDto>(fakeTimesheet))
                .Returns(new TimesheetResponseDto
                {
                    TimesheetId = fakeTimesheet.TimesheetId,
                    Description = fakeTimesheet.Description
                });

            var command = new DeleteTimesheetCommand(fakeTimesheet.TimesheetId, userId);
            var handler = new DeleteTimesheetCommandHandler(mockRepo.Object, _mapper.Object, _logger);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(fakeTimesheet.TimesheetId, result.TimesheetId);
            Assert.Equal(fakeTimesheet.Description, result.Description);

            mockRepo.Verify(r => r.GetByIdAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.DeleteTimesheetAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFailIfTimesheetNotFound()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetByIdAsync(timesheetId, CancellationToken.None))
                .ThrowsAsync(new GuidFoundException($"Timesheet com id {timesheetId} não encontrado."));

            var query = new DeleteTimesheetCommand(timesheetId, userId);
            var handler = new DeleteTimesheetCommandHandler(mockRepository.Object, _mapper.Object, _logger);

            await Assert.ThrowsAsync<GuidFoundException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteUnauthorizedIfTokenInvalid()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var differentUserId = Guid.NewGuid();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];

            mockRepository.Setup(repo => repo.GetByIdAsync(fakeTimesheet.TimesheetId, CancellationToken.None))
                .ReturnsAsync(fakeTimesheet);

            var command = new DeleteTimesheetCommand(fakeTimesheet.TimesheetId, differentUserId);
            var handler = new DeleteTimesheetCommandHandler(mockRepository.Object, _mapper.Object, _logger);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(command, CancellationToken.None)
            );

            mockRepository.Verify(repo => repo.DeleteTimesheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Delete_And_Return_Mapped_Dto_When_Valid()
        {
            var userId = Guid.NewGuid();
            var fakeTimesheet = CreateFakeTimesheet(userId);

            var command = new DeleteTimesheetCommand(fakeTimesheet.TimesheetId, userId);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);

            _repositoryMock
                .Setup(r => r.DeleteTimesheetAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapper
                .Setup(m => m.Map<TimesheetResponseDto>(fakeTimesheet))
                .Returns(new TimesheetResponseDto
                {
                    TimesheetId = fakeTimesheet.TimesheetId,
                    Description = fakeTimesheet.Description
                });

            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.TimesheetId.Should().Be(fakeTimesheet.TimesheetId);

            _repositoryMock.Verify(r =>
                r.GetByIdAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()),
                Times.Once);

            _repositoryMock.Verify(r =>
                r.DeleteTimesheetAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()),
                Times.Once);

            _mapper.Verify(m =>
                m.Map<TimesheetResponseDto>(fakeTimesheet),
                Times.Once);

        }

        [Fact]
        public async Task Handle_Should_Throw_TimesheetException_When_Not_Found()
        {
            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Timesheet?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<TimesheetException>(() =>
                handler.Handle(new DeleteTimesheetCommand(timesheetId, userId), CancellationToken.None));

            _repositoryMock.Verify(r =>
                r.DeleteTimesheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _mapper.Verify(m =>
                m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccess_When_User_Different()
        {
            var realUserId = Guid.NewGuid();
            var differentUserId = Guid.NewGuid();

            var fakeTimesheet = CreateFakeTimesheet(realUserId);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(fakeTimesheet.TimesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);

            var handler = CreateHandler();

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(new DeleteTimesheetCommand(fakeTimesheet.TimesheetId, differentUserId),
                CancellationToken.None));

            exception.Message.Should().Be(Messages.Auth.UnauthorizedAction);

            _repositoryMock.Verify(r =>
                r.DeleteTimesheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _mapper.Verify(m =>
                m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()),
                Times.Never);
        }

    }
}
