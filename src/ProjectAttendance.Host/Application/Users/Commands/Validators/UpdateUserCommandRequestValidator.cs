using FluentValidation;
using ProjectAttendance.Host.Application.Users.Commands.Requests;

namespace ProjectAttendance.Host.Application.Users.Commands.Validators
{
    public class UpdateUserCommandRequestValidator : AbstractValidator<UpdateUserCommandRequest>
    {
        public UpdateUserCommandRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Id do usuário inválido.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email inválido");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Nome de usuário inválido");

            RuleFor(x => x.Login).NotEmpty().WithMessage("Login inválido");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha inválida");
        }
    }
}