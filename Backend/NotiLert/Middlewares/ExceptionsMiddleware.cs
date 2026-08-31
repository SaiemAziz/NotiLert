using Microsoft.AspNetCore.Mvc;

namespace NotiLert.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);          // everything downstream runs inside this
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                if (context.Response.HasStarted) throw;   // too late to rewrite the response
                await HandleErrorInResponse(context, ex);
            }
        }

        private async Task HandleErrorInResponse(HttpContext context, Exception ex)
        {
            context.Response.Clear();                     // safe: HasStarted already checked
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Instance = context.Request.Path,
                Detail = _env.IsDevelopment() ? ex.ToString() : null   // never leak in Production
            };

            await context.Response.WriteAsJsonAsync(
                problem,
                options: null,
                contentType: "application/problem+json");
        }
    }
}
