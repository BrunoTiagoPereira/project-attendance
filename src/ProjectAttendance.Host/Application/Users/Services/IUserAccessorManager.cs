using ProjectAttendance.Domain.Users.Contracts;
using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.Application.Users.Services
{
    public interface IUserAccessorManager
    {
        long GetCurrentUserId();

        void ThrowIfUserDontHasAccess(IUserRelated entity);

        Task<User> GetCurrentUser();
    }
}