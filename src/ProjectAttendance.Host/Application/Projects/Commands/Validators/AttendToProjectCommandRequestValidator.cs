using FluentValidation;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;

namespace ProjectAttendance.Host.Application.Users.Commands.Validators
{
    public class AttendToProjectCommandRequestValidator : AbstractValidator<AttendToProjectCommandRequest>
    {
        public AttendToProjectCommandRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Id do usuário inválido.");

            RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("Id do projeto inválido.");

            RuleFor(x => x.StartedAt).NotEqual(default(DateTime)).WithMessage("Data de início inválida.");

            RuleFor(x => x.StartedAt).LessThan(DateTime.Now).WithMessage("A data de início deve ser menor que agora.");

            RuleFor(x => x.StartedAt).LessThan(x => x.EndedAt).WithMessage("A data de início deve ser menor que a data fim.");

            RuleFor(x => x.EndedAt).NotEqual(default(DateTime)).WithMessage("Data fim inválida.");

            RuleFor(x => x.EndedAt).LessThan(DateTime.Now).WithMessage("A data fim deve ser menor que agora.");

            RuleFor(x => x.EndedAt).GreaterThan(x => x.StartedAt).WithMessage("A data fim deve ser maior que a data de início.");
        }
    }
}