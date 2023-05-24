using Host.Api.Core.Data.Repositories;
using Host.Api.Domain.Users.Entities;
using Host.Api.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Host.Api.Infra.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public Task<bool> LoginIsTakenAsync(string login)
        {
            return Set.AnyAsync(x => x.Login == login);
        }

        public Task<User?> FindByLoginAsync(string login)
        {
            return Set
                .SingleOrDefaultAsync(x => x.Login == login)
                ;
        }

        public Task<bool> EmailIsTakenAsync(string email)
        {
            return Set.AnyAsync(x => x.Email.Value == email);
        }
    }
}