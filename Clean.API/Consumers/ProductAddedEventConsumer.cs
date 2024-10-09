using App.Domain.Events;
using MassTransit;

namespace Clean.API.Consumers
{
    public class ProductAddedEventConsumer : IConsumer<ProductAddedEvent>
    {
        public Task Consume(ConsumeContext<ProductAddedEvent> context)
        {
             Console.WriteLine($"Incoming Message:{context.Message.Id}-{context.Message.Name}-{context.Message.Price}");
            return Task.CompletedTask;
        }
    }
}
