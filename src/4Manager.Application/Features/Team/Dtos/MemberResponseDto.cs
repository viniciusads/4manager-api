using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Features.Team.Dtos
{
    public class MemberResponseDto
    {
        public List<TeamCollaborator> TeamCollaborator {  get; set; } = new List<TeamCollaborator>();
    }
}
