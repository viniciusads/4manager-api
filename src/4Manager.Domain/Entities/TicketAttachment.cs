namespace _4Tech._4Manager.Domain.Entities
{
    public class TicketAttachment
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public Guid TicketId { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
