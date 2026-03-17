using FluentValidation;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Validators
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
