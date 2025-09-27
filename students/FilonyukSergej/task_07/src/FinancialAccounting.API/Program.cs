using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5050);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddResponseCaching();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

var app = builder.Build();

// Добавим простой endpoint для проверки
app.MapGet("/", () => "API is running! Use /api/test or /api/expenses");

app.UseResponseCaching();
app.UseAuthorization();
app.MapControllers();

// Добавим обработку Ctrl+C
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    Console.WriteLine("Application is shutting down...");
    cts.Cancel();
    eventArgs.Cancel = true;
};

Console.WriteLine("Application is starting on http://localhost:5050");
Console.WriteLine("Available endpoints:");
Console.WriteLine("  GET  /api/test");
Console.WriteLine("  GET  /api/expenses");
Console.WriteLine("  POST /api/expenses");
Console.WriteLine("Press Ctrl+C to stop");

try
{
    await app.RunAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Application stopped");
}
