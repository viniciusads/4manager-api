using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<SignUpUserCommandHandler> _logger;

        public SignUpUserCommandHandler(IAuthService authService, IUserRepository repository, IMapper mapper, ILogger<SignUpUserCommandHandler> logger)
        {
            _authService = authService;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponseDto> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.SignUpAsync(request.Email, request.Password);

            var user = new UserProfile
            {
                UserId = authResult.UserId,
                Name = request.Name,
                IsActive = true
            };

            await _repository.AddUserAsync(user, cancellationToken);

            _logger.LogInformation(Messages.User.SignUpSuccess);

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
