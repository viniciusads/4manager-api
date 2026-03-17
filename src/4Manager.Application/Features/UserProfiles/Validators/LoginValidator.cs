using FluentValidation;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequestCommand>
    {
        public LoginValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email informado é inválido.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .Matches("[A-Z]").WithMessage("A senha deve conter ao menos uma letra maiúscula.")
                .Matches("[0-9]").WithMessage("A senha deve conter ao menos um número.")
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");
        }
    }
}
 