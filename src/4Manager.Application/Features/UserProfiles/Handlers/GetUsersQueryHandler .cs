using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Features.UserProfiles.Queries;
using _4Tech._4Manager.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserResponseDto>>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        public GetUsersQueryHandler(IUserRepository repository, ILogger<GetUsersQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponseDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllAsync(cancellationToken);

            _logger.LogInformation(Messages.User.Found);

            var pagedUsers = users
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return pagedUsers.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Name = u.Name,
                IsActive = u.IsActive,
                Position = u.Position,
                Function = u.Function.ToString()
            });
        }
    }
}
