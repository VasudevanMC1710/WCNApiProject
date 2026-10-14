using Microsoft.EntityFrameworkCore;
using WCNApi.Applications.Services.Implementations;
using WCNApi.core.Data;
using WCNApi.core.Data.Repositories;

namespace WCNApi.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // ----------------------------
            // Database
            // ----------------------------
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //services.AddScoped<IDbContext>(provider =>
            //    provider.GetRequiredService<AppDbContext>());


            // =========================================================
            // APPLICATION SERVICES
            // Example:
            // IUserService     -> UserService
            // IProspectService -> ProspectService
            // =========================================================

            services.Scan(scan => scan
                .FromAssemblyOf<UserService>()

                .AddClasses(classes => classes
                    .Where(type =>
                        type.Name.EndsWith(
                            "Service",
                            StringComparison.Ordinal)))

                .AsImplementedInterfaces()

                .WithScopedLifetime());


            // =========================================================
            // REPOSITORIES
            // Example:
            // IUserRepository     -> UserRepository
            // IProspectRepository -> ProspectRepository
            // =========================================================

            services.Scan(scan => scan
                .FromAssemblyOf<UserRepository>()

                .AddClasses(classes => classes
                    .Where(type =>
                        type.Name.EndsWith(
                            "Repository",
                            StringComparison.Ordinal)))

                .AsImplementedInterfaces()

                .WithScopedLifetime());


            return services;
        }
    }
}
