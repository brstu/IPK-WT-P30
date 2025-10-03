using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.ResponseCaching;
using System.Text.Json;
using Polly;
using Polly.Extensions.Http;
using Task07.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Кэширование ответов
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 64 * 1024 * 1024; // 64 MB
    options.UseCaseSensitivePaths = false;
});

// Сжатие ответов
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Настройка JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Polly для устойчивости HTTP-вызовов
builder.Services.AddHttpClient<ExternalService>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetTimeoutPolicy());

// Остальные сервисы...
builder.Services.AddControllers();
builder.Services.AddScoped<IItemService, ItemService>();

var app = builder.Build();

// Middleware pipeline
app.UseResponseCompression();
app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl = 
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(60)
        };
    
    await next();
});

app.MapControllers();

app.Run();

// Политики Polly
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => !msg.IsSuccessStatusCode)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after {timespan} seconds");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
{
    return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));
}