using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUserRepository userRepository, IAuthService authService, IMapper mapper)
        {
            _userRepository = userRepository;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            await _authService.UpdatePasswordAsync(user.UserId, request.Password);
           
            await _userRepository.UpdateUserAsync(user, cancellationToken);
            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
