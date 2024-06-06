using Microsoft.Extensions.Primitives;

namespace AspNetCore.Application.Middlewares
{
    //CorrelationMiddleware amacı ile gelen isteklerin takibini sağlamak için kullanılır ve her istek için bir oluşturur.
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        const string correlationIdHeaderKey = "X-Correlation-ID";

        public async Task Invoke(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
        {
            string correlationId = Guid.NewGuid().ToString();

            //Header içerisinde correlationId yoksa oluşturulur.
            if (context.Request.Headers.TryGetValue(correlationIdHeaderKey, out StringValues _correlationId))
                correlationId = _correlationId;
            else
                context.Request.Headers.Add(correlationIdHeaderKey, correlationId);

            //"CorrelationId" nlog.config içerisindeki keydir. 
            NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId);

            logger.LogDebug("Asp.NET Core App CorrelationId log");
            context.Response.OnStarting(() =>
            {
                if (context.Response.Headers.TryGetValue(correlationIdHeaderKey, out _))
                    context.Response.Headers.Add(correlationIdHeaderKey, correlationId);
                return Task.CompletedTask;
            });

            context.Items["CorrelationId"] = correlationId;
            await next(context);


        }
    }
}