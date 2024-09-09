using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Repositories.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                var connStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

                opt.UseSqlServer(connStrings!.SqlServer, sqlServerOptionsAction =>
                {
                    // i want it to migration to the class library here 
                    sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
                });
            });
            return services;
        }

    }
}
