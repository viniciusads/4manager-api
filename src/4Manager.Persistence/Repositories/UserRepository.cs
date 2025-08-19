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

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.", nameof(id));

            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);

            if (user == null)
                throw new UserNotFoundException($"Usuário com id {id} não encontrado.");

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email inválido.", nameof(email));

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
                throw new UserNotFoundException($"Usuário com email {email} não encontrado.");

            return user;
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "O usuário não pode ser nulo.");

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "O usuário não pode ser nulo.");

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.", nameof(id));

            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);

            if (user == null)
                throw new UserNotFoundException($"Usuário com id {id} não encontrado.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
