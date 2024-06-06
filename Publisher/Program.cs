using MassTransit;
using NLog.Extensions.Logging;
using Publisher.Services;
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq ((context, _configurator) =>
    {
        //appsettings.json dan alýnmalý
        _configurator.Host("amqps://cgjbwxmq:GikU8sJjItWnokq2HTm2MYwvpOIDP-5V@cow.rmq2.cloudamqp.com/cgjbwxmq");
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

builder.Services.AddHostedService<PublishMessageService>(provider =>
{
    using IServiceScope scope = provider.CreateScope();
    IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();

    var logger = scope.ServiceProvider.GetService<ILogger<PublishMessageService>>();
    return new(publishEndpoint, logger);
});
var host = builder.Build();
host.Run();
