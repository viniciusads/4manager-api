using System.Security.Claims;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _4Tech._4Manager.API.Controllers
{

    [ApiController]
    [Route("api/v1/timesheet")]
    [Authorize]
    public class TimesheetController : ControllerBase
    {
        private readonly IMediator _mediator;
        private Guid AuthenticatedUserId => Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value!
        );

        public TimesheetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimesheetResponseDto>>> GetAllByDateRange(DateTime startDate, DateTime endDate)
        {
            var timesheets = await _mediator.Send(new GetTimesheetsByPeriodQuery(startDate, endDate, AuthenticatedUserId));
            return Ok(timesheets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimesheetResponseDto?>> GetById(Guid id)
        {
            var timesheet = await _mediator.Send(new GetTimesheetByIdQuery(id, AuthenticatedUserId));
            return Ok(timesheet);
        }

        [HttpPost("start")]
        public async Task<ActionResult<TimesheetResponseDto>> StartTimerTimesheet([FromBody] StartTimerTimesheetCommand command)
        {
            command.AuthenticatedUserId = AuthenticatedUserId;
            var startedTimesheet = await _mediator.Send(command);
            return Ok(startedTimesheet);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> StopTimerTimesheet(Guid id, [FromBody] StopTimerTimesheetCommand command)
        {
            command.TimesheetId = id;
            command.AuthenticatedUserId = AuthenticatedUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<TimesheetResponseDto>> CreateManualTimesheet([FromBody] CreateManualTimesheetCommand command)
        {
            command.AuthenticatedUserId = AuthenticatedUserId;
            var createdTimesheet = await _mediator.Send(command);
            return Ok(createdTimesheet);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TimesheetResponseDto>> UpdateTimesheet(Guid id, [FromBody] UpdateTimesheetCommand command)
        {
            command.TimesheetId = id;
            command.AuthenticatedUserId = AuthenticatedUserId;
            var updatedTimesheet = await _mediator.Send(command);
            return Ok(updatedTimesheet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TimesheetResponseDto>> DeleteTimesheet(Guid id)
        {
            var command = new DeleteTimesheetCommand(id, AuthenticatedUserId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }        
    }
} 
