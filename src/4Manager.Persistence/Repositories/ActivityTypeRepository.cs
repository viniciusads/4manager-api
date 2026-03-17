using _4Manager.Persistence.Context;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _4Tech._4Manager.Persistence.Repositories
{
    public class ActivityTypeRepository : IActivityTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypeRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<ActivityType>> GetAllActivityTypesAsync(CancellationToken cancellationToken)
        {
            return await _context.ActivityTypes
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<ActivityType> GetActivityTypeById(Guid activityTypeId, CancellationToken cancellationToken)
        {
            var activityType = await _context.ActivityTypes.FindAsync(activityTypeId, cancellationToken);
            return activityType;
        }

        public async Task<ActivityType> GetFirstActiveActivityTypeAsync(CancellationToken cancellationToken)
        {
            return await _context.ActivityTypes
                .AsNoTracking()
                .Where(a => a.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
