using System.Text.Json;
using Task07.Api.Middleware;
using Task07.Api.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configure Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    options.MimeTypes = new[]
    {
        "application/json",
        "application/xml",
        "text/plain",
        "text/html",
        "text/css",
        "application/javascript",
        "image/svg+xml"
    };
});

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = false; // Убираем форматирование для производительности
    });

builder.Services.AddResponseCaching();

// Configure HttpClient with Polly
builder.Services.AddHttpClient("ExternalService", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "Task07.Api");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetTimeoutPolicy());

builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression(); // Включаем сжатие
app.UseResponseCaching();

// Add ETag middleware (простая реализация)
app.Use(async (context, next) =>
{
    await next();
    
    if (context.Response.StatusCode == 200 && 
        context.Response.Body != null &&
        context.Request.Method == "GET")
    {
        // Простая ETag логика для демонстрации
        var etag = Guid.NewGuid().ToString("N");
        context.Response.Headers.ETag = etag;
        
        if (context.Request.Headers.IfNoneMatch == etag)
        {
            context.Response.StatusCode = 304;
            context.Response.ContentLength = 0;
        }
    }
});

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

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
    return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
}