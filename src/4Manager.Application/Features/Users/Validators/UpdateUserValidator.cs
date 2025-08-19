using _4Tech._4Manager.Application.Features.Users.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator() {

            RuleFor(u => u.Password)
              .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
               .Matches("[A-Z]").WithMessage("A senha deve conter ao menos uma letra maiúscula.")
              .Matches("[0-9]").WithMessage("A senha deve conter ao menos um número.");
        }
    }
}
