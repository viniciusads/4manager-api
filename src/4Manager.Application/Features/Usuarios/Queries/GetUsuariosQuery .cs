using _4Manager.Application.Features.Usuarios.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Manager.Application.Features.Usuarios.Queries
{
    public class GetUsuariosQuery : IRequest<IEnumerable<UsuarioDto>>
    {
    }
}
