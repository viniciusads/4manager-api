using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Manager.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SenhaHash { get; set; } = null!;
        public bool Ativo { get; set; }
    }
}
