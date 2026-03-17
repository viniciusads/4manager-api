using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddUserAsync(UserProfile user, CancellationToken cancellationToken);      
        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateUserAsync(UserProfile user, CancellationToken cancellationToken);
        Task<UserProfile> UpdateUserProfilePictureAsync(Guid id, string userProfilePicture, CancellationToken cancellationToken);
        Task<string> GetUserNameByIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
