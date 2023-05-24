using FluentValidation;
using Host.Api.Application.Users.Commands.Requests;
using Host.Api.Core.Transaction;
using Host.Api.Core.Validators;
using Host.Api.Domain.Projects.Repositories;
using Host.Api.Domain.Users.Managers;
using Host.Api.Domain.Users.Repositories;
using Host.Api.Infra;
using Host.Api.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Host.Api.CrossCutting.Core
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

            services
                .AddScoped<IUserAccessorManager, UserAccessorManager>()
                ;

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateUserCommandRequest).Assembly);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCoreServices()
                .AddDomainServices()
                .AddDataServices(configuration)
                .AddValidators()
                ;

            return services;
        }
    }
}