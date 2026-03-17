using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Features.UserProfiles.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponseDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            _logger.LogInformation("O usuário encontrado foi o {userId}", request.UserId);

            if (user == null)
                throw new UserException();

            return user is null ? null :
               _mapper.Map<UserResponseDto>(user);
        }
    }
}
