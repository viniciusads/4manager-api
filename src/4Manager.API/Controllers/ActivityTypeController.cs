using _4Tech._4Manager.Application.Features.ActivityTypes.Queries;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Tech._4Manager.API.Controllers
{
    [ApiController]
    [Route("api/v1/activityType")]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivityTypeController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityType>>> GetAllActivityTypes() {
            var activityTypes = await _mediator.Send(new GetActivityTypesQuery());
            return Ok(activityTypes);
        }
    }
}
