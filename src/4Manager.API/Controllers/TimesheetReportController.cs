using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.TimesheetReports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _4Tech._4Manager.API.Controllers
{
    [ApiController]
    [Route("api/v1/report")]
    [Authorize]
    public class TimesheetReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private Guid AuthenticatedUserId => Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value!
        );

        public TimesheetReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getReportData")]
        public async Task<ActionResult<IEnumerable<TimesheetReportDataGroupResponseDto>>> GetReportByDataRange(DateTime startDate, DateTime endDate)
        {
            var timesheets = await _mediator.Send(new GetTimesheetReportByDataRangeQuery(startDate, endDate, AuthenticatedUserId));
            return Ok(timesheets);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportReport(DateTime startDate, DateTime endDate, string format)
        {
            var result = await _mediator.Send(
                new ExportTimesheetReportQuery(startDate, endDate, AuthenticatedUserId, format)
            );

            return File(result.FileContent, result.ContentType, result.FileName);
        }
    }
}
