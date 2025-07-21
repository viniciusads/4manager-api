using _4Manager.Domain.Entities;
using _4Manager.Domain.Interfaces;
using _4Manager.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace _4Manager.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Usuarios.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
