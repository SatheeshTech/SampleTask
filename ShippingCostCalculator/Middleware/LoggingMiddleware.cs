namespace ShippingCostCalculator.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation($"Request: {httpContext.Request.Method} {httpContext.Request.Path}");
            await _next(httpContext);
            _logger.LogInformation($"Response: {httpContext.Response.StatusCode}");
        }
    }
}
