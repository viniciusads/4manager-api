namespace _4Tech._4Manager.Application.Features.Projects.Dtos
{
    public class TicketAttachmentResposeDto
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
