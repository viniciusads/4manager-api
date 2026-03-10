using _4tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public record GetProjectByIdQuery(Guid projectId) : IRequest<ProjectResponseDto>
    {
        public Guid projectId { get; set; } = projectId;
    }
}
