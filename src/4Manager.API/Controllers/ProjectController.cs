using _4Tech._4Manager.API.Options;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace _4Tech._4Manager.API.Controllers
{
    [ApiController]
    [Route("api/v1/project")]
    public class ProjetoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PaginationOptions _paginationOptions;

        public ProjetoController(IMediator mediator, IOptions<PaginationOptions> paginationOptions)
        {
            _mediator = mediator;
            _paginationOptions = paginationOptions.Value;
        }

        [HttpGet]
        public async Task<ActionResult<PagedProjectResponseDto>> GetAll(
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            var page = (pageNumber.HasValue && pageNumber.Value > 0)
                ? pageNumber.Value
                : _paginationOptions.DefaultPageNumber;

            var size = (pageSize.HasValue && pageSize.Value > 0)
                ? pageSize.Value
                : _paginationOptions.DefaultPageSize;

            var max = _paginationOptions.MaxPageSize > 0
                ? _paginationOptions.MaxPageSize
                : 100;

            size = Math.Min(size, max);

            var query = new GetPagedProjectsQuery { PageNumber = page, PageSize = size };
            var result = await _mediator.Send(query);

            return Ok(result);
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
        public async Task<ActionResult<ProjectResponseDto>> Delete(Guid id)
        {
            var command = new DeleteProjectCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
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
        public async Task<ActionResult<ProjectResponseDto>> Create([FromBody] CreateProjectCommand command)
        {
            var projectId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = projectId }, projectId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> Update(Guid id, [FromBody] UpdateProjectCommand command)
        {
            command.ProjectId = id;
            var updatedProject = await _mediator.Send(command);
            return Ok(updatedProject);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ProjectResponseDto>> UpdateStatus(Guid id, [FromBody] UpdateStatusCommand command)
        {
            command.ProjectId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
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
