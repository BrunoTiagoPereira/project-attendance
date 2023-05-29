using FluentValidation;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;

namespace ProjectAttendance.Host.Application.Users.Commands.Validators
{
    public class CreateProjectCommandRequestValidator : AbstractValidator<CreateProjectCommandRequest>
    {
        public CreateProjectCommandRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Título inválido.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Descrição inválida.");
        }
    }
}