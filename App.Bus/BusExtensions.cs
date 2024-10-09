using App.Application.Contracts.ServiceBus;
using App.Bus;
using App.Domain.Consts;
using App.Domain.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App_Bus
{
    public static class BusExtensions
    {
        public static void AddBusExt(this IServiceCollection services ,IConfiguration configuration)
        {
            var serviceBusOption = configuration.GetSection(nameof(ServiceBusOption)).Get<ServiceBusOption>();

            services.AddScoped<IServiceBus,ServiceBus>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductAddedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(serviceBusOption!.Url), h => { });

                    cfg.ReceiveEndpoint(ServiceBusConst.ProductAddedEventQeueName, e =>
                    {
                        e.ConfigureConsumer<ProductAddedEventConsumer>(context);
                    });
                });
            });



        }
    }
}
