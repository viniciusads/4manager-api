using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Manager.Application.Features.Users.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Active { get; set; } 
    }
}
