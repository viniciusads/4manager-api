using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Commands
{
    public class UpdateProjectCommand : IRequest<ProjectResponseDto>
    {

        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime DeliveryDate { get; set; }
        public Guid? CustomerId { get; set; }
        public string TitleColor { get; set; } = string.Empty;
        public List<Guid> CollaboratorIds { get; set; } = new List<Guid>();

        public UpdateProjectCommand(Guid projectId, Guid? customerId, string projectName, string titleColor, List<Guid> collaboratorIds, DateTime deliveryDate)
        {
            ProjectId = projectId;
            CustomerId = customerId;
            DeliveryDate = deliveryDate;
            ProjectName = projectName;
            TitleColor = titleColor;
            CollaboratorIds = collaboratorIds;
        }
    }
}
