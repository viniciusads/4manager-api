using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Manager.Application.Features.Usuarios.Dtos
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
