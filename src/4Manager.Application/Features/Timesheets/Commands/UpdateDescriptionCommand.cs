using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Commands
{
    public class UpdateDescriptionCommand : IRequest<TimesheetResponseDto>
    {
        public Guid TimesheetId { get; set; }
        public string Description { get; set; }

        public UpdateDescriptionCommand() { }

        public UpdateDescriptionCommand(string description, Guid timesheetId)
        {
            Description = description;
            TimesheetId = timesheetId;
        }
    }
}
