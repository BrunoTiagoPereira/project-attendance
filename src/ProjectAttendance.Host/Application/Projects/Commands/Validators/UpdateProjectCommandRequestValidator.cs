using FluentValidation;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;

namespace ProjectAttendance.Host.Application.Users.Commands.Validators
{
    public class UpdateProjectCommandRequestValidator : AbstractValidator<UpdateProjectCommandRequest>
    {
        public UpdateProjectCommandRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Título inválido.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Descrição inválida.");
        }
    }
}