using System.Diagnostics;
namespace EF_core_practice_API.middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path;

            _logger.LogInformation("Request started {Method} {Path}", method, path);

            await _next(context);

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;

            _logger.LogInformation(
                "Request finished {Method} {Path} -> {StatusCode} in {ElapsedMilliseconds} ms",
                method,
                path,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
