using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Tech._4Manager.API.Controllers
{
    [ApiController]
    [Route("api/v1/project")]
    public class ProjetoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjetoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll()
        {
            var projects = await _mediator.Send(new GetProjectsQuery());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetByStatus(ProjectStatusEnum status)
        {
            var projects = await _mediator.Send(new GetProjectsByStatusQuery(status));
            return Ok(projects);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteProjectCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}/ticket")]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetByProjectIdAsync(Guid id)
        {
            var projects = await _mediator.Send(new GetTicketsByProjectIdQuery(id));
            return Ok(projects);
        }

        [HttpGet("{ticketId}/ticketDetails")]
        public async Task<ActionResult> GetByTicketId(Guid ticketId)
        {
            var projects = await _mediator.Send(new GetTicketDetailsByTicketIdQuery(ticketId));
            return Ok(projects);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProjectCommand command)
        {
            var projectId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = projectId }, projectId);
        }

        [HttpPost("{projectId}/ticket")]
        public async Task<ActionResult<Guid>> CreateTicket(Guid projectId, [FromBody] CreateTicketCommand command)
        {
            command.ProjectId = projectId;
            var ticketId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByTicketId), new { ticketId = ticketId }, ticketId);
        }

        [HttpPost("{ticketDetailsId}/ticketNote")]
        public async Task<ActionResult<Guid>> CreateTicketNote(Guid ticketDetailsId, [FromBody] CreateTicketNoteCommand command)
        {
            command.TicketDetailsId = ticketDetailsId;
            var noteId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByTicketId), new { ticketId = noteId }, noteId);
        }
    }
}
