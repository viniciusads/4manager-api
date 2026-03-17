using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Domain.Enums;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Commands
{
    public class UpdateStatusCommand : IRequest<ProjectResponseDto>
    {
        public Guid ProjectId { get; set; }
        public ProjectStatusEnum StatusProject { get; set; }

        public UpdateStatusCommand(Guid projectId, ProjectStatusEnum statusProject)
        {
            ProjectId = projectId;
            StatusProject = statusProject;
        }

    }
}
