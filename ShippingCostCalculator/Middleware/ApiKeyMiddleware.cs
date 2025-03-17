using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ShippingCostCalculator.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private const string ApiKeyHeaderName = "X-Api-Key";
        private readonly string _validApiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            // Load the valid API key from configuration
            _validApiKey = configuration.GetValue<string>("ApiKey");
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Check if the request is for Swagger-related resources (bypass API Key check)
            var path = context.Request.Path;
            if (path.StartsWithSegments("/swagger") || path.StartsWithSegments("/favicon.ico"))
            {
                // Allow Swagger through without API Key check
                await _next(context);
                return;
            }
            // Check if the request includes the API Key
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                _logger.LogWarning("API Key was not provided.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            // Validate the API Key
            if (!_validApiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("Invalid API Key provided.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
                //await context.Response.WriteAsync("Invalid API Key.");
                return;
            }

            // If the API Key is valid, proceed to the next middleware
            await _next(context);
        }
    }
}
