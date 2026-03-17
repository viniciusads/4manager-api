using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Domain.Entities
{
    public class MessageHistory
    {
        public Guid MessageHistoryId { get; set; }
        public MessageStatusEnum MessageStatus { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; }
        public Guid TicketDetailsId { get; set; }
    }
}
