using Microsoft.EntityFrameworkCore;
using ProjectAttendance.Core.Data.Repositories;
using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Domain.Users.Repositories;

namespace ProjectAttendance.Infra.Repositories
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