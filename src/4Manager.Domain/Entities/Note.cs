namespace _4Tech._4Manager.Domain.Entities
{
    public class Note
    {
        public Guid NoteId { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public Guid TicketDetailsId { get; set; }
    }
}
