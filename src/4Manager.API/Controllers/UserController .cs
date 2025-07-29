using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Queries;
using _4Manager.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Manager.API.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("criar-usuario")]
        public async Task<ActionResult<CreateUserDto>> Create(CreateUserCommand command)
        {
            var createdUser = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserCredentialsCommand command)
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpPut("resetar-senha")]
        public async Task<ActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }



    }
}
