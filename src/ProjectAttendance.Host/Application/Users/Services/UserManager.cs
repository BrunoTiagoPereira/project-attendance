using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Responses;
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
            _secret = configuration.GetSection("JwtToken:Secret").Value;
        }

        public Task<AuthenticateCommandResponse> Authenticate(AuthenticateCommandRequest auth)
        {
            throw new NotImplementedException();
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

        public Task<GetUserQueryResponse> GetUserById(long id)
        {
            throw new NotImplementedException();
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