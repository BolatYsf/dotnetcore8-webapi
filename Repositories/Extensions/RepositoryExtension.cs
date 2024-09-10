using App.Repositories.Products;
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

            // should use scoped lifecycle cause u use dbcontext so after scoped than dispose instance
            services.AddScoped<IProductRepository,ProductRepository>();

            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));


            return services;
        }

    }
}
