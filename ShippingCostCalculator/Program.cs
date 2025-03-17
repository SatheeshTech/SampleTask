using ShippingCostCalculator.Data.Models;
using Microsoft.EntityFrameworkCore;
using ShippingCostCalculator.Data.Interface;
using ShippingCostCalculator.Data.Repository;
using ShippingCostCalculator.ServiceLayer;
using ShippingCostCalculator.Middleware;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
     {
         policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
     });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ShippingCostCalculator", Version = "v1" });
    // Setup for API Key authentication
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey, // Specify API Key type
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,   // API Key will be passed in the header
        Name = "X-Api-Key",                                        // The header name for the API Key
        Description = "API Key must be provided to access endpoints.",
    });

    // Secure the API by requiring the API Key for specified endpoints
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "ApiKey",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<IShippingCostCalculatorContext, ShippingCostCalculatorContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IShippingCostCalculatorRepository, ShippingCostCalculatorRepository>();
builder.Services.AddScoped<IShippingCalculatorService, ShippingCalculatorService>();

builder.Services.AddMemoryCache();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
// Custom middleware: Timing and logging
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyMiddleware>();
});
app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
// CORS processing for Cross-Origin support

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.UseAuthorization();
app.MapControllers();


app.Run();
