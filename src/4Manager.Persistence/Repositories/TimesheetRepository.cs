using _4Manager.Persistence.Context;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                .Include(t => t.Customer)
                .Include(t => t.Project).ThenInclude(p => p.Customer)
                .Include(t => t.ActivityType)
                .Where(t => t.UserId == userId)
                .Where(t => t.Date >= startDay && t.Date <= endDay)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheetReportByDateRangeAsync(DateTime startDate, DateTime endDate, Guid userId, CancellationToken cancellationToken)
        {
            var startDay = startDate.Date;
            var endDay = endDate.Date;

            return await _context.Timesheets
                .AsNoTracking()
                .Include(t => t.ActivityType)
                .Where(t => t.UserId == userId)
                .Where(t => t.Date >= startDay && t.Date <= endDay)
                .ToListAsync(cancellationToken);
        }

        public async Task<Timesheet?> GetByIdAsync(Guid TimesheetId, CancellationToken cancellationToken)
        {
            return await _context.Timesheets
                .Include(t => t.Customer)
                .Include(t => t.Project)
                .Include(t => t.ActivityType)
                .FirstOrDefaultAsync(t => t.TimesheetId == TimesheetId, cancellationToken);
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

        public async Task<Timesheet> StopTimerTimesheetAsync(Guid timesheetId, DateTime endDate, string description, Guid? projectId, CancellationToken cancellationToken)
        {
            var timesheet = await _context.Timesheets.FirstOrDefaultAsync(t => t.TimesheetId == timesheetId, cancellationToken);

            timesheet.EndDate = endDate;
            timesheet.Description = description;
            timesheet.ProjectId = projectId;

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

        public async Task<TimeSpan> GetTotalTimeByProjectAndUserAsync(Guid projectId, Guid userId, CancellationToken cancellationToken)
        {
            var timesheets = await _context.Timesheets
                .Where(t =>
                    t.ProjectId == projectId &&
                    t.UserId == userId &&
                    t.EndDate != null
                )
                .ToListAsync(cancellationToken);

            var totalSeconds = timesheets
                .Sum(t => (t.EndDate.Value - t.StartDate).TotalSeconds);

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public async Task<Dictionary<Guid, TimeSpan>> GetTotalTimeByProjectsAndUserAsync(List<Guid> projectIds, Guid userId, CancellationToken cancellationToken)
        {
            if (projectIds == null || projectIds.Count == 0)
                return new Dictionary<Guid, TimeSpan>();

            var timesheets = await _context.Timesheets
                .AsNoTracking()
                .Where(t =>
                    t.ProjectId.HasValue &&
                    projectIds.Contains(t.ProjectId.Value) &&
                    t.UserId == userId &&
                    t.EndDate.HasValue
                )
                .Select(t => new
                {
                    ProjectId = t.ProjectId.Value,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate!.Value
                })
                .ToListAsync(cancellationToken);

            var totals = timesheets
                .GroupBy(t => t.ProjectId)
                .Select(g => new
                {
                    ProjectId = g.Key,
                    TotalTicks = g.Sum(t => (t.EndDate - t.StartDate).Ticks)
                })
                .ToList();

            return totals.ToDictionary(
                t => t.ProjectId,
                t => TimeSpan.FromTicks(t.TotalTicks)
            );
        }
    }
}
