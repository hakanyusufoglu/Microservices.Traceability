using NLog;

namespace AspNetCore.Application.Middlewares
{
    public class OtherMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context, ILogger<OtherMiddleware> logger)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            //ya da örnek amaçlı olduğu için aşağıdaki kodda aynı değeri üreteceğinden yorum satırına alındı
            //correlationId = context.Items["CorrelationId"].ToString();

            MappedDiagnosticsContext.Set("CorrelationId", correlationId);
            logger.LogDebug("OtherMiddleware log");

            await next(context);
        }
    }
}
