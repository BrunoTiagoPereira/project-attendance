using FluentValidation;
using ProjectAttendance.Host.Application.Projects.Queries.Requests;

namespace ProjectAttendance.Host.Application.Projects.Queries.Validators
{
    public class GetProjectQueryRequestValidator : AbstractValidator<GetProjectQueryRequest>
    {
        public GetProjectQueryRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Requisição inválida.");

            RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("Projeto inválido.");
        }
    }
}