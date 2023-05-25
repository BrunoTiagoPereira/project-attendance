using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.Domain.Users.Contracts
{
    public interface IHasUsersRelated
    {
        IReadOnlyCollection<User> Users { get; }
    }
}