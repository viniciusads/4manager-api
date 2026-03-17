using FluentValidation;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Validators
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
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
                 .Matches("[A-Z]").WithMessage("A senha deve conter ao menos uma letra maiúscula.")
                .Matches("[0-9]").WithMessage("A senha deve conter ao menos um número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Deve conter pelo menos 1 caractere especial");

            RuleFor(u => u.ConfirmPassword)
               .NotEmpty().WithMessage("Confirmação da senha é obrigatória.")
               .Equal(u => u.Password).WithMessage("As senhas não coincidem.");
        }
    }
}
