using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken);
        Task<IEnumerable<Project>> GetByStatusAsync(ProjectStatusEnum status, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);

        Task CreateProjectAsync(Project project, CancellationToken cancellationToken);
        Task UpdateProjectAsync(Project project, CancellationToken cancellationToken);
        Task<Project> UpdateStatusAsync(Guid projectId, ProjectStatusEnum statusProject, CancellationToken cancellationToken);
        Task<RoleEnum?> GetManagerRoleAsync(Guid managerId, CancellationToken cancellationToken);
        Task<bool> CollaboratorsExistAsync(IEnumerable<Guid> collaboratorIds, CancellationToken cancellationToken);
        Task<bool> ProjectNameExistsAsync(string projectName, CancellationToken cancellationToken);
        Task<List<int>> GetManagersRolesAsync(List<Guid> managerIds, CancellationToken cancellationToken); 
        Task DeleteProjectAsync(Guid id, CancellationToken cancellationToken);
    }
}
