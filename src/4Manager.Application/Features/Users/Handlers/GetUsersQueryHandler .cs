using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Queries;
using _4Manager.Application.Interfaces;
using MediatR;


namespace _4Manager.Application.Features.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserResponseDto>>
    {
        private readonly IUserRepository _repository;

        public GetUsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserResponseDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _repository.GetAllAsync(cancellationToken);

            return usuarios.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                isActive = u.isActive,
                Role = u.Role.ToString()
            });
        }
    }
}
