using FluentValidation;
using ProjectAttendance.Host.Application.Projects.Queries.Requests;

namespace ProjectAttendance.Host.Application.Projects.Queries.Validators
{
    public class GetProjectsFromUserQueryRequestValidator : AbstractValidator<GetProjectsFromUserQueryRequest>
    {
        public GetProjectsFromUserQueryRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Requisição inválida.");
        }
    }
}