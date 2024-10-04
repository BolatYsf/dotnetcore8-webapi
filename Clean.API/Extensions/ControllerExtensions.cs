using Clean.API.Filter;
using Clean.API.Filters;

namespace Clean.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddControllersWithFiltersExt(this IServiceCollection services)
        {

            services.AddScoped(typeof(NotFoundFilter<,>));

            services.AddControllers(opt =>
            {
                opt.Filters.Add<FluentValidationFilter>();
                opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes=true;
            });

            return services;
        }
    }
}
