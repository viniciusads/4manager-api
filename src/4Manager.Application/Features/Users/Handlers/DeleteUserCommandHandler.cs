using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public DeleteUserCommandHandler(IUserRepository userRepository, IAuthService authService, IMapper mapper)
        {
            _userRepository = userRepository;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<UserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            
            await _authService.SoftDeleteUserAsync(request.UserId, cancellationToken);

            return _mapper.Map<UserResponseDto>(user);

        }
    }
}
