using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {

        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("A senha deve conter ao menos uma letra maiúscula.")
                .Matches("[0-9]").WithMessage("A senha deve conter ao menos um número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Deve conter pelo menos 1 caractere especial");

        }        
    }
}