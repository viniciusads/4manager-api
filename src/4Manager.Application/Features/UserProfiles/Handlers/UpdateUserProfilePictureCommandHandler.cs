using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class UpdateUserProfilePictureCommandHandler : IRequestHandler<UpdateUserProfilePictureCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserProfilePictureCommandHandler> _logger;

        public UpdateUserProfilePictureCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserProfilePictureCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponseDto> Handle(UpdateUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var UpdatedUserProfilePicture = await _userRepository.UpdateUserProfilePictureAsync(request.UserId, request.UserProfilePicture, cancellationToken);

            _logger.LogInformation(Messages.User.PhotoUpdated);

            return _mapper.Map<UserResponseDto>(UpdatedUserProfilePicture);
        }
    }
}
