using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using Bogus;

namespace _4Tech._4Manager.Application.Tests.Fixtures
{
    public class TicketDetailsTestFixture
    {
        private readonly Faker<Note> _noteFaker;
        private readonly Faker<MessageHistory> _messageHistoryFaker;
        private readonly Faker<TicketDetails> _ticketDetailsFaker;
        private readonly TicketTestFixture _ticketFixture;

        public TicketDetailsTestFixture()
        {
            _noteFaker = new Faker<Note>()
                .RuleFor(n => n.NoteId, f => f.Random.Guid())
                .RuleFor(n => n.NoteText, f => f.Lorem.Paragraph())
                .RuleFor(m => m.TicketDetailsId, f => Guid.Empty);

            _messageHistoryFaker = new Faker<MessageHistory>()
                .RuleFor(m => m.MessageHistoryId, f => f.Random.Guid())
                .RuleFor(m => m.MessageStatus, f => f.PickRandom<MessageStatusEnum>())
                .RuleFor(m => m.Subject, f => f.Lorem.Sentence(6))
                .RuleFor(m => m.Sender, f => f.Person.FullName)
                .RuleFor(m => m.MessageDate, f => f.Date.Recent())
                .RuleFor(m => m.TicketDetailsId, f => Guid.Empty);

            _ticketDetailsFaker = new Faker<TicketDetails>()
                .RuleFor(td => td.TicketDetailsId, f => f.Random.Guid())
                .RuleFor(td => td.TicketId, f => f.Random.Guid())
                .RuleFor(td => td.Ticket, f => null!)
                .RuleFor(td => td.Note, f => _noteFaker.Generate(f.Random.Int(1, 3)))
                .RuleFor(td => td.MessageHistory, f => _messageHistoryFaker.Generate(f.Random.Int(1, 3)));
        }
        public TicketDetails GenerateTestTicketDetails()
        {
            return _ticketDetailsFaker.Generate();
        }
    }
}
