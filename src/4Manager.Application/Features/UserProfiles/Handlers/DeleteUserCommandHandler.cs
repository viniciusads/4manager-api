using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        public DeleteUserCommandHandler(IUserRepository userRepository, IAuthService authService, IMapper mapper, ILogger<DeleteUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user.Function != RoleEnum.Gestor)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedAction);

            await _authService.SoftDeleteUserAsync(request.UserId, cancellationToken);

            _logger.LogInformation(Messages.User.Deleted);

            return _mapper.Map<UserResponseDto>(user);

        }
    }
}
