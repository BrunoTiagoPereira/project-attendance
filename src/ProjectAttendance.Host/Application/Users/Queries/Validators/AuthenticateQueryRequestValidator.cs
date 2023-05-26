using FluentValidation;

namespace ProjectAttendance.Host.Application.Users.Queries.Requests
{
    public class AuthenticateQueryRequestValidator : AbstractValidator<AuthenticateQueryRequest>
    {
        public AuthenticateQueryRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Requisição inválida.");

            RuleFor(x => x.Login).NotEmpty().WithMessage("Login inválido.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha inválida.");
        }
    }
}