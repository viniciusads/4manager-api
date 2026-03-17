using _4Manager.Persistence.Context;
using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace _4Tech._4Manager.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Projects
                .AsNoTracking()
                .OrderBy(p => p.ProjectName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(projectId, cancellationToken);
            return project;
        }

        public async Task<IEnumerable<Project>> GetByStatusAsync(ProjectStatusEnum status, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Projects
                .AsNoTracking()
                .Where(p => p.StatusProject == status)
                .OrderBy(p => p.ProjectName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _context.Projects.CountAsync(cancellationToken);
        }
        public async Task CreateProjectAsync(Project project, CancellationToken cancellationToken)
        {
            await _context.Projects.AddAsync(project, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task UpdateProjectAsync(Project project, CancellationToken cancellationToken)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Project> UpdateStatusAsync(Guid projectId, ProjectStatusEnum statusProject,  CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(t => t.ProjectId == projectId, cancellationToken);

            project.StatusProject = statusProject;
            project.ProjectId = projectId;

            await _context.SaveChangesAsync(cancellationToken);
            return project;
        }

        public async Task<RoleEnum?> GetManagerRoleAsync(Guid managerId, CancellationToken cancellationToken)
        {
            return await _context.UserProfiles
                .Where(u => u.UserId == managerId && u.IsActive)
                .Select(u => (RoleEnum?)u.Function)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> CollaboratorsExistAsync(IEnumerable<Guid> collaboratorIds, CancellationToken cancellationToken)
        {
            var collaboratorIdsList = collaboratorIds?.ToList() ?? new List<Guid>();
            
            if (collaboratorIdsList.Count == 0)
                return true;

            var count = await _context.UserProfiles
                .Where(u => collaboratorIdsList.Contains(u.UserId) && u.IsActive)
                .CountAsync(cancellationToken);

            return count == collaboratorIdsList.Count;
        }

        public async Task<List<int>> GetManagersRolesAsync(List<Guid> managerIds, CancellationToken cancellationToken)
        {
            return await _context.UserProfiles
                .Where(u => managerIds.Contains(u.UserId))
                .Select(u => (int)u.Function)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ProjectNameExistsAsync(string projectName, CancellationToken cancellationToken)
        {
             if (string.IsNullOrWhiteSpace(projectName))
                return false;

            return await _context.Projects
                .AnyAsync(p => EF.Functions.ILike(p.ProjectName, projectName.Trim()), cancellationToken);
        }
        public async Task DeleteProjectAsync(Guid id, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id, cancellationToken);

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
