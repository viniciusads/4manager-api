using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Domain.Enums;
using MediatR;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;

namespace _4tech._4Manager.Application.Features.Users.Handlers
{
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public SignUpUserCommandHandler(IAuthService authService, IUserRepository repository, IMapper mapper)
        {
            _authService = authService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.SignUpAsync(request.Email, request.Password);

            var user = new User
            {
                UserId = authResult.UserId,
                Name = request.Name,
                Email = request.Email,
                IsActive = true,
                Role = RoleEnum.Analista
            };

            await _repository.AddUserAsync(user, cancellationToken);

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
