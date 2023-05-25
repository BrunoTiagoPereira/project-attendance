using ProjectAttendance.Domain.Users.Entities;
using ProjectAttendance.Host.Application.Users.Commands.Requests;
using ProjectAttendance.Host.Application.Users.Services;
using ProjectAttendance.Infra;

namespace ProjectAttendance.Host.Seed
{
    public class DatabaseSeed : IDatabaseSeed
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void InitializeAndSeedDatabase()
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();

            context.Database.EnsureCreated();

            if (!HasAdminUserAlready(context))
            {
                userManager.CreateUser(new CreateUserCommandRequest
                {
                    Username = "admin",
                    Password = "admin123",
                    Email = "admin@admin.com.br",
                    Login = "admin"
                });
            }
        }

        private static bool HasAdminUserAlready(DatabaseContext context)
        {
            return context.Set<User>().Any(x => x.Username == "admin");
        }
    }
}
