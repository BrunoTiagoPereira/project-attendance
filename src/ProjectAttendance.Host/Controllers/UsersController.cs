using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Commands.Responses;
using ProjectAttendance.Host.Application.Users.Queries.Requests;
using ProjectAttendance.Host.Application.Users.Queries.Responses;
using ProjectAttendance.Host.Application.Users.Services;

namespace ProjectAttendance.Host.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public Task<AuthenticateQueryResponse> Authenticate(AuthenticateQueryRequest request)
        {
            return _userManager.Authenticate(request);
        }

        [HttpPost]
        [Route("users")]
        public Task<CreateUserCommandResponse> CreateUser(CreateUserCommandRequest request)
        {
            return _userManager.CreateUser(request);
        }

        [HttpPost]
        [Route("users/{id}")]
        public Task<CreateUserCommandResponse> GetUser(CreateUserCommandRequest request)
        {
            return _userManager.GetUserById(request);
        }
    }
}