using App.Services.Categories;
using App.Services.ExceptionHandlers;
using App.Services.Filter;
using App.Services.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<ICategoryService, CategoryService>();

            // if u wanna use async validation at validationfilter u have to close below this code
            services.AddFluentValidationAutoValidation();

            services.AddScoped(typeof(NotFoundFilter<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddExceptionHandler<CriticalExceptionHandler>();

            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}
