using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using Bogus;

namespace _4Tech._4Manager.Application.Tests.Fixtures
{
    public class TicketTestFixture
    {
        private readonly Faker<TicketAttachment> _attachmentFaker;
        private readonly Faker<TicketDetails> _ticketDetailsFaker;
        private readonly Faker<Ticket> _ticketFaker;

        public TicketTestFixture()
        {
            _attachmentFaker = new Faker<TicketAttachment>()
                .RuleFor(a => a.AttachmentId, f => f.Random.Guid())
                .RuleFor(a => a.FileName, f => f.System.FileName())
                .RuleFor(a => a.FilePath, f => f.System.FilePath())
                .RuleFor(a => a.FileSize, f => f.Random.Long(1000, 1000000))
                .RuleFor(a => a.UploadDate, f => f.Date.Recent());

            _ticketDetailsFaker = new Faker<TicketDetails>()
                .RuleFor(d => d.TicketDetailsId, f => f.Random.Guid())
                .RuleFor(d => d.Note, new List<Note>())
                .RuleFor(d => d.MessageHistory, new List<MessageHistory>());

            _ticketFaker = new Faker<Ticket>()
                .RuleFor(t => t.TicketId, f => f.Random.Guid())
                .RuleFor(t => t.ProjectId, f => Guid.NewGuid())
                .RuleFor(t => t.TicketNumber, f => f.Random.Int(1, 9999))
                .RuleFor(t => t.InternalCall, f => f.Random.Guid())
                .RuleFor(t => t.Applicant, f => f.Name.FullName())
                .RuleFor(t => t.Sector, f => f.Commerce.Department())
                .RuleFor(t => t.TicketResponsible, f => f.Name.FullName())
                .RuleFor(t => t.TicketStatus, f => f.PickRandom<TicketStatusEnum>())
                .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                .RuleFor(t => t.Priority, f => f.PickRandom<TicketPriorityEnum>())
                .RuleFor(t => t.OpeningDate, f => f.Date.Recent(30))
                .RuleFor(t => t.DeadlineDate, f => f.Date.Future(1))
                .RuleFor(t => t.AffectedSystem, f => f.Commerce.ProductName())
                .RuleFor(t => t.ResponsibleArea, f => f.Commerce.Department())
                .RuleFor(t => t.Attachments, f => _attachmentFaker.Generate(f.Random.Int(1, 5)))

                .RuleFor(t => t.TicketDetails, (f, t) =>
                {
                    var details = _ticketDetailsFaker.Generate();
                    details.TicketId = t.TicketId;
                    details.Ticket = t;
                    return details;
                });
        }

        public List<Ticket> GenerateTestTickets(int count)
        {
            return _ticketFaker.Generate(count);
        }
    }
}
