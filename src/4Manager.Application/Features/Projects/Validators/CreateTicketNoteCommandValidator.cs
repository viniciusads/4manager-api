using _4Tech._4Manager.Application.Features.Projects.Commands;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class CreateTicketNoteCommandValidator : AbstractValidator<CreateTicketNoteCommand>
    {
        public CreateTicketNoteCommandValidator()
        {
            RuleFor(x => x.TicketDetailsId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O TicketDetailsId inserido não é um Guid válido.");

            RuleFor(x => x.NoteText)
                .NotEmpty().WithMessage("O texto da nota é obrigatório.")
                .MaximumLength(500).WithMessage("A nota não pode ter mais de 500 caracteres.");
        }
    }
}
