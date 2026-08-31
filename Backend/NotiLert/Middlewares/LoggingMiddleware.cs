using System.Diagnostics;

namespace NotiLert.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/openapi"))
            {
                await _next(context);
                return;
            }
            long start = Stopwatch.GetTimestamp();
            try
            {
                await _next(context);
            }
            finally
            {
                TimeSpan elapsed = Stopwatch.GetElapsedTime(start);
                int status = context.Response.StatusCode;

                LogLevel level = status >= 500 ? LogLevel.Error
                               : status >= 400 ? LogLevel.Warning
                               : LogLevel.Information;


                _logger.Log(level, "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs:0.0} ms",
                    context.Request.Method, context.Request.Path, status, elapsed.TotalMilliseconds);
            }
        }
    }
}
