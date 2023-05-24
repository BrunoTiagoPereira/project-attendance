using Host.Api.Domain.Users.Contracts;
using Host.Api.Domain.Users.Entities;

namespace Host.Api.Domain.Users.Managers
{
    public interface IUserAccessorManager
    {
        long GetCurrentUserId();

        void ThrowIfUserDontHasAccess(IUserRelated entity);

        Task<User> GetCurrentUser();
    }
}