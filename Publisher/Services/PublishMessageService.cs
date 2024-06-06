using MassTransit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Publisher.Services
{
    public class PublishMessageService(IPublishEndpoint publishEndpoint, ILogger<PublishMessageService> logger) : BackgroundService
    {
        //Publisher loglanabiliyor
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var correlationId = Guid.NewGuid();

            int i = 0;

            while (true)
            {
                ExampleMessage message = new()
                {
                    Text = $"{++i} .mesaj"
                };

                Trace.CorrelationManager.ActivityId = correlationId;
                logger.LogDebug("Publisher Log");

                await Console.Out.WriteLineAsync($"{JsonSerializer.Serialize(message)} - Correlation Id : {correlationId}");

                await publishEndpoint.Publish(message, async context =>
                {
                    context.Headers.Set("CorrelationId", correlationId);
                });

                await Task.Delay(750);
            }
        }
    }
}
