using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Tech._4Manager.API.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> Get()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user == null)
            { return NotFound(); }
            return Ok(user);
        }


        [HttpPost("sign-up")]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] SignUpUserCommand command)
        {
            var createdUser = await _mediator.Send(command);
            if (createdUser == null)
            {
                return BadRequest(new { message = "Falha ao cadastrar usuário" });
            }
            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> Login([FromBody] LoginRequestCommand command)
        {

            var loginResult = await _mediator.Send(command);

            return Ok(loginResult);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}/update-user")]
        public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.UserId = id;
            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        [HttpPatch("change-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordCommand command)
        {
            var updatedPassword = await _mediator.Send(command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/update-image")]
        public async Task<ActionResult<TimesheetResponseDto>> InsertUserProfilePicture(Guid id, [FromBody] UpdateUserProfilePictureCommand command)
        {
            command.UserId = id;
            var userProfilePicture = await _mediator.Send(command);
            return Ok(userProfilePicture);
        }
    }
}
