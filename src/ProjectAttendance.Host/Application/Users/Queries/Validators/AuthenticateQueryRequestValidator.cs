using FluentValidation;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Extensions;
using ProjectAttendance.Core.ValueObjects;
using ProjectAttendance.Domain.Users.Repositories;

namespace ProjectAttendance.Host.Application.Users.Queries.Requests
{
    public class AuthenticateQueryRequestValidator : AbstractValidator<AuthenticateQueryRequest>
    {
        private readonly IUserRepository _userRepository;

        public AuthenticateQueryRequestValidator(IUserRepository userRepository) : this()
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        private AuthenticateQueryRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Requisição inválida.");

            RuleFor(x => x.Login).NotEmpty().WithMessage("Login inválido.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha inválida.");

            When(x => x.Login.IsNotNullOrWhiteSpace() && x.Password.IsNotNullOrWhiteSpace(), () =>
            {
                RuleFor(x => x).Must((x, y) => IsUserValid(x)).WithMessage("Usuário não encontrado");
            });
        }


        private bool IsUserValid(AuthenticateQueryRequest request)
        {
            var user = _userRepository.FindByLoginAsync(request.Login).GetAwaiter().GetResult();

            if (user is null)
            {
                return false;
            }

            var isPasswordValid = new Password(request.Password).Hash == user.Password.Hash;

            if (!isPasswordValid)
            {
                return false;
            }

            return true;
        }
    }
}