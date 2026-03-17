using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using FluentAssertions;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class CreateTicketNoteCommandHandlerTests : IClassFixture<TicketDetailsTestFixture>
    {
        private readonly TicketDetailsTestFixture _fixture;
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly CreateTicketNoteCommandHandler _handler;

        public CreateTicketNoteCommandHandlerTests(TicketDetailsTestFixture fixture)
        {
            _fixture = fixture;
            _ticketRepositoryMock = new Mock<ITicketRepository>();

            _handler = new CreateTicketNoteCommandHandler(_ticketRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateNoteWhenValid()
        {
            var ticketDetails = _fixture.GenerateTestTicketDetails();
            var noteText = "Testando criação de nota";

            var command = new CreateTicketNoteCommand
            {
                TicketDetailsId = ticketDetails.TicketDetailsId,
                NoteText = noteText
            };

            _ticketRepositoryMock
                .Setup(r => r.TicketDetailsExistsAsync(ticketDetails.TicketDetailsId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();

            _ticketRepositoryMock.Verify(
                r => r.CreateTicketNoteAsync(
                    It.Is<Note>(n =>
                        n.TicketDetailsId == ticketDetails.TicketDetailsId &&
                        n.NoteText == noteText
                    ),
                    It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task NotFoundWhenTicketDetailsIdDoesntExist()
        {
            var command = new CreateTicketNoteCommand
            {
                TicketDetailsId = Guid.NewGuid(),
                NoteText = "Alguma nota"
            };

            _ticketRepositoryMock
                .Setup(r => r.TicketDetailsExistsAsync(command.TicketDetailsId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ActivityException>();
        }
    }
}