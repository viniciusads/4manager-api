using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
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
    public class GetTimesheetByIdQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapper = new();
        private readonly TimesheetTestFixture _fixture;
        private readonly Mock<ITimesheetRepository> _repositoryMock = new();
        private readonly ILogger<GetTimesheetByIdQueryHandler> _logger;

        private GetTimesheetByIdQueryHandler CreateHandler()
            => new GetTimesheetByIdQueryHandler(
                _repositoryMock.Object,
                _mapper.Object,
                _logger);

        private static Timesheet CreateFakeTimesheet(Guid userId)
        {
            return new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                UserId = userId,
                Description = "Teste"
            };
        }

        public GetTimesheetByIdQueryHandlerTests()
        {
          
            _logger = new Mock<ILogger<GetTimesheetByIdQueryHandler>>().Object;
            _fixture = new TimesheetTestFixture();
        }

        [Fact]
        public async Task GetTimesheetByRequiredId()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];


            mockRepository.Setup(repo => repo.GetByIdAsync(fakeTimesheet.TimesheetId, CancellationToken.None))
                .ReturnsAsync(fakeTimesheet);

            _mapper
                .Setup(m => m.Map<TimesheetResponseDto>(fakeTimesheet))
                .Returns(new TimesheetResponseDto
                {
                    TimesheetId = fakeTimesheet.TimesheetId,
                    Description = fakeTimesheet.Description
                });

            var handler = new GetTimesheetByIdQueryHandler(mockRepository.Object, _mapper.Object, _logger);

            var result = await handler.Handle(new GetTimesheetByIdQuery(fakeTimesheet.TimesheetId, fakeTimesheet.UserId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(fakeTimesheet.TimesheetId, result.TimesheetId);
        }

        [Fact]
        public async Task GetTimesheetByRequiredIdFailIfTimesheetNotFound()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var fakeTimesheet = _fixture.GeneratesTimesheet(1)[0];

            mockRepository.Setup(repo => repo.GetByIdAsync(fakeTimesheet.TimesheetId, CancellationToken.None))
                .ThrowsAsync(new GuidFoundException($"Timesheet com id {fakeTimesheet.TimesheetId} não encontrado."));

            var handler = new GetTimesheetByIdQueryHandler(mockRepository.Object, _mapper.Object, _logger);
            var query = new GetTimesheetByIdQuery(fakeTimesheet.TimesheetId, fakeTimesheet.UserId);

            var exceptionMessage = $"Timesheet com id {fakeTimesheet.TimesheetId} não encontrado.";
            var exception = await Assert.ThrowsAsync<GuidFoundException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal(exceptionMessage, exception.Message);
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
                handler.Handle(
                    new GetTimesheetByIdQuery(fakeTimesheet.TimesheetId, differentUserId),
                    CancellationToken.None));

            exception.Message.Should().Be(Messages.Auth.UnauthorizedView);

            _mapper.Verify(m =>
                m.Map<TimesheetResponseDto>(It.IsAny<Timesheet>()),
                Times.Never);
        }

    }
}