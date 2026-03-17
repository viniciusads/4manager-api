using _4Tech._4Manager.Application.Interfaces;
using _4Manager.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Application.Common.Exceptions;

namespace _4Manager.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.UserProfiles.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<UserProfile> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _context.UserProfiles.FindAsync(new object[] { id }, cancellationToken);
            return user;
        }

        public async Task AddUserAsync(UserProfile user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "O usuário não pode ser nulo.");

            await _context.UserProfiles.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(UserProfile user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "O usuário não pode ser nulo.");

            _context.UserProfiles.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.", nameof(id));

            var user = await _context.UserProfiles.FindAsync(new object[] { id }, cancellationToken);

            if (user == null)
                throw new UserException($"Usuário com id {id} não encontrado.");

            _context.UserProfiles.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserProfile> UpdateUserProfilePictureAsync(Guid id, string userProfilePicture, CancellationToken cancellationToken)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == id, cancellationToken);

            user.UserProfilePicture = userProfilePicture;

            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<string> GetUserNameByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var name = await _context.Set<UserProfile>()
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync(cancellationToken);

            return name ?? "Usuário sem nome.";
        }
    }
}
