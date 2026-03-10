using _4Tech._4Manager.Application.Tests.Fixtures;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class TicketDetailsFixtureTests
    {
        private readonly TicketDetailsTestFixture _fixture;

        public TicketDetailsFixtureTests()
        {
            _fixture = new TicketDetailsTestFixture();
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
    }
}
