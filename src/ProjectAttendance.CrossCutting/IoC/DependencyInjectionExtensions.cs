using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Projects.Repositories;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Infra;
using ProjectAttendance.Infra.Repositories;

namespace ProjectAttendance.CrossCutting.IoC
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IValidatorManager, ValidatorManager>();

            return services;
        }

        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<DbContext, DatabaseContext>(builder =>
            {
                builder.UseSqlite(configuration.GetConnectionString("Sqlite"));
            }, ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                ;

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCoreServices()
                .AddDomainServices()
                .AddDataServices(configuration)
                ;

            return services;
        }
    }
}