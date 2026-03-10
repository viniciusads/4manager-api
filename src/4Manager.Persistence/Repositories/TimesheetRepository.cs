using _4Manager.Persistence.Context;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _4Tech._4Manager.Persistence.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly ApplicationDbContext _context;

        public TimesheetRepository(ApplicationDbContext context){
            _context = context;           
        }

        public async Task<IEnumerable<Timesheet>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid userId, CancellationToken cancellationToken)
        {
            var startDay = startDate.Date;
            var endDay = endDate.Date;

            return await _context.Timesheets
                .AsNoTracking()
                .Where(t => t.UserId == userId) 
                .Where(t => t.Date >= startDay && t.Date <= endDay)
                .ToListAsync(cancellationToken);
        }

        public async Task<Timesheet> GetByIdAsync(Guid TimesheetId, CancellationToken cancellationToken)
        {
            var timesheet = await _context.Timesheets.FindAsync(TimesheetId, cancellationToken);
            return timesheet;
        }

        public async Task StartTimerTimesheetAsync(Timesheet timesheet, CancellationToken cancellationToken)
        {
            await _context.Timesheets.AddAsync(timesheet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateManualTimesheetAsync(Timesheet timesheet, CancellationToken cancellationToken)
        {
            await _context.Timesheets.AddAsync(timesheet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Timesheet> StopTimerTimesheetAsync(Guid timesheetId, DateTime endDate, string description, CancellationToken cancellationToken)
        {
            var timesheet = await _context.Timesheets.FirstOrDefaultAsync(t => t.TimesheetId == timesheetId, cancellationToken);

            timesheet.EndDate = endDate;
            timesheet.Description = description;

            await _context.SaveChangesAsync(cancellationToken);
            return timesheet;
        }

        public async Task DeleteTimesheetAsync(Guid timesheetId, CancellationToken cancellationToken)
        {
            var timesheet = await _context.Timesheets.FindAsync(timesheetId, cancellationToken);

            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTimesheetAsync(Timesheet timesheet, CancellationToken cancellationToken)
        {
            _context.Timesheets.Update(timesheet);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
