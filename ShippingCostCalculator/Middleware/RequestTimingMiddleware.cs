using System.Diagnostics;

namespace ShippingCostCalculator.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Start timing
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            // Stop timing 
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            _logger.LogInformation($"Request to {context.Request.Method} {context.Request.Path} took {elapsedMilliseconds} ms.");
        }
    }
}
