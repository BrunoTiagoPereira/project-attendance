using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.Domain.Users.Contracts
{
    public interface IUserRelated
    {
        User User { get; }
        long UserId { get; }
    }
}