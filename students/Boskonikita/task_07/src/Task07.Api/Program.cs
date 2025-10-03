using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Сервисы
builder.Services.AddResponseCaching();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Пайплайн
app.UseResponseCompression();
app.UseResponseCaching();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ЗАКОММЕНТИРУЙТЕ HTTPS редирект - он вызывает ошибку
// app.UseHttpsRedirection();

// Глобальная обработка ошибок
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var problemDetails = new
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Status = 500,
            Detail = ex.Message,
            Instance = context.Request.Path,
            TraceId = context.TraceIdentifier
        };
        
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
});

app.MapControllers();

Console.WriteLine("Application starting...");
Console.WriteLine("Press Ctrl+C to stop");

app.Run();

public partial class Program { }