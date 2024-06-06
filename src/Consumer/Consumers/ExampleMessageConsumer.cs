using MassTransit;
using Shared;
using System.Diagnostics;

namespace Consumer.Consumers
{
    // Bu sınıfın amacı, ExampleMessage tipindeki mesajları dinlemek ve işlemektir.
    public class ExampleMessageConsumer(ILogger<ExampleMessageConsumer> logger) : IConsumer<ExampleMessage>
    {
        public Task Consume(ConsumeContext<ExampleMessage> context)
        {
            var correlationId = Guid.NewGuid();
            if(context.Headers.TryGetHeader("CorrelationId", out var _correlationId))
                correlationId = Guid.Parse(_correlationId.ToString());

            Trace.CorrelationManager.ActivityId = correlationId;
            logger.LogDebug("Consumer Log");

            Console.WriteLine($"Receive message : {context.Message.Text} - Correlation Id : {correlationId}");

            return Task.CompletedTask;
        }
    }
}
