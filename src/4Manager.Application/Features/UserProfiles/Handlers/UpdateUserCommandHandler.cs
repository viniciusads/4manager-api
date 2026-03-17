using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Supabase.Gotrue;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly Supabase.Client _client;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, Supabase.Client client, ILogger<UpdateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _client = client;
            _logger = logger;
        }
        public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            await ValidateBusinessRulesAsync(request, cancellationToken);

            await ChangeEmailAsync(request);
            
            if (!string.IsNullOrWhiteSpace(request.Name))
                user.Name = request.Name;

            if (request.Position.HasValue)
                user.Position = request.Position.Value;

            await _userRepository.UpdateUserAsync(user, cancellationToken);

            _logger.LogInformation(Messages.User.ProfileUpdated);

            return _mapper.Map<UserResponseDto>(user);
        }
        private async Task<Unit> ValidateBusinessRulesAsync(UpdateUserCommand request, CancellationToken cancellationToken) {

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                throw new UserException();

            if (user.UserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedAction);

            return Unit.Value;
        }

        private async Task<Unit> ChangeEmailAsync(UpdateUserCommand request)
        {
            var currentUser = _client.Auth.CurrentUser;
            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != currentUser.Email)
            {
                await _client.Auth.SetSession(request.AccessToken, request.RefreshToken);
                if (request.AccessToken == null)
                    throw new Exception("O AccessToken não foi encontrado");

                if (request.RefreshToken == null)
                    throw new Exception("O RefreshToken não foi encontrado");

                var attrs = new UserAttributes
                {
                    Email = request.Email
                };
                await _client.Auth.Update(attrs);
            }
            return Unit.Value;
        }
    }
}
