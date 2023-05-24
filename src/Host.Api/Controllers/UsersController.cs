using Host.Api.Application.Users.Commands.Requests;
using Host.Api.Application.Users.Commands.Responses;
using Host.Api.Application.Users.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Api.Controllers
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
        public Task<AuthenticateCommandResponse> Authenticate(AuthenticateCommandRequest request)
        {
            return _userManager.Authenticate(request);
        }
    }
}