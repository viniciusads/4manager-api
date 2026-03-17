using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Features.Projects.Dtos
{
    public class TicketDetailsResponseDto
    {
        public Guid TicketId { get; set; }
        public Guid TicketDetailsId { get; set; }
        public List<Note> Note { get; set; } = new List<Note>();
        public List<MessageHistory> MessageHistory { get; set; } = new List<MessageHistory>();
    }
}
