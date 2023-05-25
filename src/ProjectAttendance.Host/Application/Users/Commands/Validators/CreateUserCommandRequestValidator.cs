using FluentValidation;
using ProjectAttendance.Core.Extensions;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Users.Commands.Requests;

namespace ProjectAttendance.Host.Application.Users.Commands.Validators
{
    public class CreateUserCommandRequestValidator : AbstractValidator<CreateUserCommandRequest>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandRequestValidator(IUserRepository userRepository) : this()
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        private CreateUserCommandRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Requisição inválida.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email inválido");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Nome de usuário inválido");

            RuleFor(x => x.Login).NotEmpty().WithMessage("Login inválido");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha inválida");

            When(x => x.Email.IsNotNullOrWhiteSpace(), () =>
            {
                RuleFor(x => x.Email).Must((x) => !IsEmailTaken(x)).WithMessage("Email já utilizado");
            });

            When(x => x.Login.IsNotNullOrWhiteSpace(), () =>
            {
                RuleFor(x => x.Email).Must((x) => !IsLoginTaken(x)).WithMessage("Login já utilizado");
            });
        }

        private bool IsEmailTaken(string email)
        {
            return _userRepository.EmailIsTakenAsync(email).GetAwaiter().GetResult();
        }
        private bool IsLoginTaken(string login)
        {
            return _userRepository.LoginIsTakenAsync(login).GetAwaiter().GetResult();
        }
    }
}
