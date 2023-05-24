using Host.Api.Domain.Users.Entities;

namespace Host.Api.Domain.Users.Contracts
{
    public interface IUserRelated
    {
        User User { get; }
        long UserId { get; }
    }
}