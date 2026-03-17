using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Interfaces
{
    public interface IActivityTypeRepository
    {
        Task<IEnumerable<ActivityType>> GetAllActivityTypesAsync(CancellationToken cancellationToken);
        Task<ActivityType> GetActivityTypeById(Guid id, CancellationToken cancellationToken);
        Task<ActivityType> GetFirstActiveActivityTypeAsync(CancellationToken cancellationToken);
    }
}
