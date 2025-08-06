using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
            { return NotFound();}
            return Ok(user);
        }
       

        [HttpPost("cadastrar")]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] SignUpUserCommand command)
        {
            var createdUser = await _mediator.Send(command);
            if (createdUser == null)
            {
                return BadRequest(new{ message = "Falha ao cadastrar usuário" });
            }
            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestCommand command)
        {

            var loginResult = await _mediator.Send(command);

            return Ok(loginResult);
        }
        
        [HttpPost("resetar-senha")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

    }
}
