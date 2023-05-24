using Host.Api.Core.Data.Repositories;
using Host.Api.Domain.Users.Entities;

namespace Host.Api.Domain.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> LoginIsTakenAsync(string login);
        Task<bool> EmailIsTakenAsync(string email);

        Task<User?> FindByLoginAsync(string login);
    }
}