using Host.Api.Domain.Users.Entities;

namespace Host.Api.Domain.Users.Contracts
{
    public interface IHasUsersRelated
    {
        IReadOnlyCollection<User> Users { get; }
    }
}