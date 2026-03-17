using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Interfaces
{
    public interface ITimesheetRepository
    {
        Task<IEnumerable<Timesheet>> GetByDateRangeAsync (DateTime startDate, DateTime endDate, Guid userId, CancellationToken cancellationToken);
        Task<Timesheet?> GetByIdAsync(Guid TimesheetId, CancellationToken cancellationToken);
        Task StartTimerTimesheetAsync(Timesheet timesheet, CancellationToken cancellationToken);
        Task CreateManualTimesheetAsync(Timesheet timesheet, CancellationToken cancellationToken);
        Task<Timesheet> StopTimerTimesheetAsync(Guid timesheetId, DateTime endDate, string description, Guid? projectId, CancellationToken cancellationToken);
        Task DeleteTimesheetAsync(Guid timesheetId, CancellationToken cancellationToken);
        Task UpdateTimesheetAsync(Timesheet timesheet, CancellationToken cancellationToken);
        Task<TimeSpan> GetTotalTimeByProjectAndUserAsync(Guid projectId, Guid userId, CancellationToken cancellationToken);
        Task<Dictionary<Guid, TimeSpan>> GetTotalTimeByProjectsAndUserAsync(List<Guid> projectIds, Guid userId, CancellationToken cancellationToken);
    }
}
