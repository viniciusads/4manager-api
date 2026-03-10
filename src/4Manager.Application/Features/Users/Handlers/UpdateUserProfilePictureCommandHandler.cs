using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class UpdateUserProfilePictureCommandHandler : IRequestHandler<UpdateUserProfilePictureCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserProfilePictureCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(UpdateUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var UpdatedUserProfilePicture = await _userRepository.UpdateUserProfilePictureAsync(request.UserId, request.UserProfilePicture, cancellationToken);
            return _mapper.Map<UserResponseDto>(UpdatedUserProfilePicture);
        }
    }
}
