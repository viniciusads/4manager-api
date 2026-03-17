using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class TicketDetailsFixtureTests
    {
        private readonly TicketDetailsTestFixture _fixture;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ITicketRepository> _ticketRepository;

        public TicketDetailsFixtureTests()
        {
            _fixture = new TicketDetailsTestFixture();
            _ticketRepository = new Mock<ITicketRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public void TicketDetailsShouldReturnValidObject()
        {
            var ticketDetails = _fixture.GenerateTestTicketDetails();

            Assert.NotNull(ticketDetails);
            Assert.NotEqual(Guid.Empty, ticketDetails.TicketDetailsId);
            Assert.NotEqual(Guid.Empty, ticketDetails.TicketId);
        }

        [Fact]
        public void TicketDetailsShouldContainNote()
        {
            var ticketDetails = _fixture.GenerateTestTicketDetails();

            Assert.NotNull(ticketDetails.Note);
            Assert.NotEmpty(ticketDetails.Note);

            foreach (var note in ticketDetails.Note)
            {
                Assert.NotEqual(Guid.Empty, note.NoteId);
                Assert.False(string.IsNullOrWhiteSpace(note.NoteText));
            }
        }

        [Fact]
        public void TicketDetailsShouldContainMessageHistory()
        {
            var ticketDetails = _fixture.GenerateTestTicketDetails();

            Assert.NotNull(ticketDetails.MessageHistory);
            Assert.NotEmpty(ticketDetails.MessageHistory);

            foreach (var message in ticketDetails.MessageHistory)
            {
                Assert.NotEqual(Guid.Empty, message.MessageHistoryId);
                Assert.False(string.IsNullOrWhiteSpace(message.Subject));
                Assert.False(string.IsNullOrWhiteSpace(message.Sender));
                Assert.NotEqual(default, message.MessageDate);
            }
        }

        [Fact]
        public void TicketDetailsShallNotBeNull()
        {
            var ticketDetails = _fixture.GenerateTestTicketDetails();

            Assert.Null(ticketDetails.Ticket);
        }

        [Fact]
        public async Task Handle_ShouldReturnTicketDetailsDto_WhenTicketExists()
        {
            var ticketId = Guid.NewGuid();

            var entity = new Ticket();
            var dto = new TicketDetailsResponseDto();

            _ticketRepository
                .Setup(x => x.GetByTicketId(ticketId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));

            _mapper
                .Setup(x => x.Map<TicketDetailsResponseDto>(entity))
                .Returns(dto);

            var handler = new GetTicketDetailsByTicketIdQueryHandler(
                _ticketRepository.Object,
                _mapper.Object);

            var query = new GetTicketDetailsByTicketIdQuery(ticketId);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task Handle_ShouldThrowGuidException_WhenTicketNotFound()
        {
            var ticketId = Guid.NewGuid();

            _ticketRepository
                .Setup(x => x.GetByTicketId(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Ticket?)null);

            var handler = new GetTicketDetailsByTicketIdQueryHandler(_ticketRepository.Object, _mapper.Object);

            var query = new GetTicketDetailsByTicketIdQuery(ticketId);

            await Assert.ThrowsAsync<GuidException>(() =>
                handler.Handle(query, CancellationToken.None));
        }
    }
}
