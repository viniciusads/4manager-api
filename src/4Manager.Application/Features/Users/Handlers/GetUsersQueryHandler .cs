using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Queries;
using _4Manager.Domain.Interfaces;
using _4Manager.Domain.Entities;
using MediatR;


namespace _4Manager.Application.Features.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _repository;

        public GetUsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _repository.GetAllAsync(cancellationToken);

            return usuarios.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email
            });
        }
    }
}
