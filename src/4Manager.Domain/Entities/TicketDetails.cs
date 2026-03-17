namespace _4Tech._4Manager.Domain.Entities
{
    public class TicketDetails
    {
        public Guid TicketDetailsId { get; set; }
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public List<Note> Note { get; set; } = new List<Note>();
        public List<MessageHistory> MessageHistory { get; set; } = new List<MessageHistory>();
    }
}
