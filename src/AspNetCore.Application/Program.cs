using AspNetCore.Application.Middlewares;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

#region NLog setup
builder.Logging.ClearProviders();
builder.Host.UseNLog();
#endregion

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<OtherMiddleware>();

app.MapGet("/", (HttpContext context, ILogger<Program> logger) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

    MappedDiagnosticsContext.Set("CorrelationId", correlationId);
    logger.LogDebug("Minimal Api Log");
});

app.Run();
