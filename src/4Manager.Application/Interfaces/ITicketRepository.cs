using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetByProjectIdAsync(Guid projectId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Ticket> GetByTicketId(Guid ticketId, CancellationToken cancellationToken);
        Task<Ticket> CreateTicketAsync(Ticket ticket, CancellationToken cancellationToken);
        Task<Note> CreateTicketNoteAsync(Note noteId, CancellationToken cancellationToken);
        Task<bool> TicketDetailsExistsAsync(Guid ticketDetailsId, CancellationToken cancellationToken);
        Task<int> GetNextTicketNumberAsync(CancellationToken cancellationToken);
    }
}
