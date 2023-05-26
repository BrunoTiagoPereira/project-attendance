using FluentValidation;

namespace ProjectAttendance.Host.Application.Users.Queries.Requests
{
    public class GetUserQueryRequestValidator : AbstractValidator<GetUserQueryRequest>
    {
        public GetUserQueryRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Requisição inválida.");

            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Usuário inválido.");
        }
    }
}