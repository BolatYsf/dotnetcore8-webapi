using App.Application.Contracts.Caching;
using App.Caching;

namespace Clean.API.Extensions
{
    public static class CachingExtensions
    {
        public static IServiceCollection CachingExt(this IServiceCollection services)
        {

            services.AddSingleton<ICacheService, CacheService>();
            services.AddMemoryCache();

            return services;
        }
    }
}
