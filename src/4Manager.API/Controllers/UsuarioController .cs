using _4Manager.Application.Features.Usuarios.Dtos;
using _4Manager.Application.Features.Usuarios.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Manager.API.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> Get()
        {
            var usuarios = await _mediator.Send(new GetUsuariosQuery());
            return Ok(usuarios);
        }
    }
}
