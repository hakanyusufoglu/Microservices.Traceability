using Consumer;
using Consumer.Consumers;
using MassTransit;
using NLog.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<ExampleMessageConsumer>();

    configurator.UsingRabbitMq((context, _configurator) =>
    {
        //appsettings.json dan alýnmalý
        _configurator.Host("amqps://cgjbwxmq:GikU8sJjItWnokq2HTm2MYwvpOIDP-5V@cow.rmq2.cloudamqp.com/cgjbwxmq");

        _configurator.ReceiveEndpoint("example-message-qeueue", endpoint =>
        {
            endpoint.ConfigureConsumer<ExampleMessageConsumer>(context);
        });
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
