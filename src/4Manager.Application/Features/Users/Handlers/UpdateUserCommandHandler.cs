using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;
using Supabase.Gotrue;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly Supabase.Client _client;
        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, Supabase.Client client)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _client = client;
        }
        public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null) 
                throw new Exception("Usuário não encontrado.");

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                await _client.Auth.SetSession(request.AccessToken, request.RefreshToken);

                if (request.AccessToken == null)
                    throw new Exception("O AccessToken não foi encontrado");

                if (request.RefreshToken == null)
                    throw new Exception("O RefreshToken não foi encontrado");

                var attrs = new UserAttributes { Email = request.Email };
                await _client.Auth.Update(attrs); 
                user.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                user.Name = request.Name;

            if (request.Position.HasValue)
                user.Position = request.Position.Value;

            await _userRepository.UpdateUserAsync(user, cancellationToken);
            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
