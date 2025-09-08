using MediatR;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Features.Users.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            return user is null ? null :
               _mapper.Map<UserResponseDto>(user);
        }
    }
}
