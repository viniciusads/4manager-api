using FluentValidation;
using _4Manager.Application.Features.Users.Commands;

namespace _4Manager.Application.Features.Users.Validators
{
    public class SignUpUserValidator : AbstractValidator<SignUpUserCommand>
    {
        public SignUpUserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

            RuleFor(u => u.ConfirmPassword)
               .NotEmpty().WithMessage("Confirmação da senha é obrigatória.")
               .Equal(u => u.Password).WithMessage("As senhas não coincidem.");
        }
    }
}
