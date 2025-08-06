using FluentValidation;
using _4Manager.Application.Features.Users.Commands;

namespace _4Manager.Application.Features.Users.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email informado é inválido.");

        }
    }
}
