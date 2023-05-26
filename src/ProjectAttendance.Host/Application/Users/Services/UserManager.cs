using Microsoft.IdentityModel.Tokens;
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
        private readonly string _secret;

        public UserManager(IValidatorManager validatorManager, IUserRepository userRepository, IUnitOfWork uow, IConfiguration configuration)
        {
            _validatorManager = validatorManager ?? throw new ArgumentNullException(nameof(validatorManager));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
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

        public async Task<CreateUserCommandResponse> CreateUser(CreateUserCommandRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var user = new User(request.Username, request.Login, request.Email, request.Password);

            await _userRepository.AddAsync(user);

            await _uow.CommitAsync();

            return new CreateUserCommandResponse
            {
                Id = user.Id,
                Email = user.Email.Value,
                Login = user.Login,
                Username = user.Username
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

        public Task<UpdateUserCommandResponse> UpdateUser(UpdateUserCommandRequest request)
        {
            throw new NotImplementedException();
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