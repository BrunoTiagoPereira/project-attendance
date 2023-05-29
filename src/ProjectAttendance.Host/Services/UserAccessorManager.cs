using ProjectAttendance.Application.Users.Services;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Domain.Users.Contracts;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;
using System.Security.Claims;

namespace ProjectAttendance.Host.Services
{
    public class UserAccessorManager : IUserAccessorManager
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserRepository _userRepository;

        public UserAccessorManager(IHttpContextAccessor accessor, IUserRepository userRepository)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Task<User> GetCurrentUser()
        {
            return _userRepository.FindAsync(GetCurrentUserId());
        }

        public long GetCurrentUserId()
        {
            return long.Parse(_accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        public void ThrowIfUserDontHasAccess(IUserRelated entity)
        {
            if (GetCurrentUserId() != entity.UserId)
            {
                throw new NotAuthorizedException("Usuário não autorizado");
            }
        }

        public void ThrowIfUserDontHasAccess(IHasUsersRelated entity)
        {
            if (!entity.Users.Any(x => x.Id == GetCurrentUserId()))
            {
                throw new NotAuthorizedException("Usuário não autorizado");
            }
        }
    }
}