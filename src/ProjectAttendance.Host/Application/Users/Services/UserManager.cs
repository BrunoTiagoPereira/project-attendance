using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectAttendance.Application.Users.Services;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Core.ValueObjects;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Responses;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using ProjectAttendance.Host.Application.Users.Queries.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectAttendance.Host.Application.Users.Services
{
    public class UserManager : IUserManager
    {
        private readonly IValidatorManager _validatorManager;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly IUserAccessorManager _userAccessorManager;
        private readonly string _secret;

        public UserManager(IValidatorManager validatorManager, IUserRepository userRepository, IUnitOfWork uow, IUserAccessorManager userAccessorManager, IConfiguration configuration)
        {
            _validatorManager = validatorManager ?? throw new ArgumentNullException(nameof(validatorManager));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userAccessorManager = userAccessorManager ?? throw new ArgumentNullException(nameof(userAccessorManager));
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _secret = configuration.GetSection("JwtToken:Secret").Value;
        }

        public async Task<AuthenticateQueryResponse> Authenticate(AuthenticateQueryRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var user = await _userRepository.FindByLoginAsync(request.Login);

            if(user is null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            var isPasswordValid = new Password(request.Password).Hash == user.Password.Hash;

            if (!isPasswordValid)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            return new AuthenticateQueryResponse
            {
                User = new AuthenticationUserResponse
                {
                    Username = user.Username,
                    Email = user.Email.Value,
                    Login = user.Login,
                    Password = user.Password.Hash
                },
                Token = GenerateToken(user)
            };
        }

        public async Task<CreateUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var emailIsTaken = await _userRepository.EmailIsTakenAsync(request.Email);
            if (emailIsTaken)
            {
                throw new DomainException("Email já cadastrado.");
            }

            var loginIsTaken = await _userRepository.LoginIsTakenAsync(request.Login);
            if (loginIsTaken)
            {
                throw new DomainException("Login já cadastrado.");
            }

            var user = new User(request.Username, request.Login, request.Email, request.Password);

            await _userRepository.AddAsync(user);

            await _uow.CommitAsync();

            return new CreateUserCommandResponse
            {
                User = new CreateUserUserCommandResponse
                {
                    Id = user.Id,
                    Email = user.Email.Value,
                    Login = user.Login,
                    Username = user.Username
                }
            };
        }

        public async Task<UpdateUserCommandResponse> UpdateUserAsync(UpdateUserCommandRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var currentUserId = _userAccessorManager.GetCurrentUserId();

            if (request.UserId != currentUserId)
            {
                throw new DomainException("O usuário não tem permissão para editar outros usuários");
            }

            var user = await _userRepository.FindAsync(request.UserId);

            if(user is null)
            {
                throw new DomainException("Usuário não existe.");
            }

            var emailIsTaken = await _userRepository.Set.AnyAsync(x => x.Email.Value == request.Email && x.Id != request.UserId);
            if (emailIsTaken)
            {
                throw new DomainException("Email já cadastrado.");
            }

            var loginIsTaken = await _userRepository.Set.AnyAsync(x => x.Login == request.Login && x.Id != request.UserId);
            if (loginIsTaken)
            {
                throw new DomainException("Login já cadastrado.");
            }

            user.UpdateLogin(request.Login);
            user.UpdateUsername(request.Username);
            user.UpdateEmail(request.Email);
            user.UpdatePassword(request.Password);

            _userRepository.Update(user);

            await _uow.CommitAsync();

            return new UpdateUserCommandResponse
            {
                User = new UpdateUserUserCommandResponse
                {
                    Id = user.Id,
                    Email = user.Email.Value,
                    Login = user.Login,
                    Username = user.Username
                }
            };
        }

        public async Task<GetUserQueryResponse> GetUser(GetUserQueryRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var user = await _userRepository.FindAsync(request.UserId);

            if(user is null)
            {
                throw new DomainException("Usuário não encontrado");
            }

            return new GetUserQueryResponse
            {
                User = new GetUserQueryUserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email.Value,
                    Login = user.Login
                }
            };
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}