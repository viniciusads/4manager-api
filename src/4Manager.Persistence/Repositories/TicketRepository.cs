using _4Manager.Persistence.Context;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _4Tech._4Manager.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private const int FirstTicketNumber = 1;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Ticket>> GetByProjectIdAsync(
            Guid projectId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("Id do projeto inválido.", nameof(projectId));

            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Attachments)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Ticket?> GetByTicketId(Guid TicketId, CancellationToken cancellationToken)
        {
            return await _context.Tickets
                    .AsNoTracking()
                    .Include(t => t.TicketDetails)
                        .ThenInclude(td => td.Note)
                    .Include(t => t.TicketDetails)
                        .ThenInclude(td => td.MessageHistory)
                    .FirstOrDefaultAsync(t => t.TicketId == TicketId, cancellationToken);
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            await _context.Tickets.AddAsync(ticket, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket;
        }

        public async Task<int> GetNextTicketNumberAsync(CancellationToken cancellationToken)
        {
            var hasAnyTicket = await _context.Tickets.AnyAsync(cancellationToken);

            if (!hasAnyTicket)
            {
                return FirstTicketNumber;
            }
            var maxNumber = await _context.Tickets
                .MaxAsync(t => t.TicketNumber, cancellationToken);

            return maxNumber + 1;
        }

        public async Task<Note?> GetTicketNoteByIdAsync(Guid noteId, CancellationToken cancellationToken)
        {
            return await _context.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NoteId == noteId, cancellationToken);
        }
        public async Task<Note> CreateTicketNoteAsync(Note note, CancellationToken cancellationToken)
        {
            await _context.Notes.AddAsync(note, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return note;
        }

        public async Task<bool> TicketDetailsExistsAsync(Guid ticketDetailsId, CancellationToken cancellationToken)
        {
            return await _context.TicketDetails
                .AnyAsync(td => td.TicketDetailsId == ticketDetailsId, cancellationToken);
        }
    }
}
