using _4Tech._4Manager.Application.Features.ActivityTypes.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.ActivityTypes.Queries
{
    public class GetActivityTypesQuery : IRequest<IEnumerable<ActivityTypeResponseDto>>
    {
        public GetActivityTypesQuery() {
        
        }
    }
}
