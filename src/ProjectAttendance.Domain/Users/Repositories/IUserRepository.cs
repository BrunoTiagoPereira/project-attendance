using ProjectAttendance.Core.Data.Repositories;
using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.Domain.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> LoginIsTakenAsync(string login);

        Task<bool> EmailIsTakenAsync(string email);

        Task<User?> FindByLoginAsync(string login);
    }
}