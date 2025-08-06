using FluentValidation;
using _4Manager.Application.Features.Users.Commands;

namespace _4Manager.Application.Features.Users.Validators
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
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
 