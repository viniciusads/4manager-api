using _4Tech._4Manager.Application.Features.ActivityTypes.Dtos;
using _4Tech._4Manager.Application.Features.ActivityTypes.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.ActivityTypes.Handler
{
    public class GetActivityTypesQueryHandler : IRequestHandler<GetActivityTypesQuery, IEnumerable<ActivityTypeResponseDto>>
    {
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly IMapper _mapper;

        public GetActivityTypesQueryHandler(IActivityTypeRepository activityTypeRepository, IMapper mapper)
        {
            _activityTypeRepository = activityTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityTypeResponseDto>> Handle(GetActivityTypesQuery request, CancellationToken cancellationToken)
        {
            var activityTypes = await _activityTypeRepository.GetAllActivityTypesAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ActivityTypeResponseDto>>(activityTypes);
        }
    }
}
