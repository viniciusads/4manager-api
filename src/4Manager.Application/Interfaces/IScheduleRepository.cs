using _4Tech._4Manager.Domain.Entities;


namespace _4Tech._4Manager.Application.Interfaces

{
    public interface IScheduleRepository
    {
        Task<Schedule> GetScheduleByIdAsync(Guid projectId, CancellationToken cancellationToken);
        Task CreateScheduleAsync(Schedule schedule, CancellationToken cancellationToken);
        Task UpdateScheduleAsync(Schedule schedule, CancellationToken cancellationToken);

        Task DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken);

        Task<bool> ScheduleExistsAsync(Guid projectId, CancellationToken cancellationToken);
    }
}