namespace _4Tech._4Manager.Application.Features.Team.Dtos
{
    public class TeamResponseDto
    {
        public Guid TeamId { get; set; }
        public string ProjectName { get; set; } = null!;
        public MemberResponseDto Members { get; set; } = null!;
    }
}
