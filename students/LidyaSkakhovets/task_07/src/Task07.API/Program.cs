using Microsoft.OpenApi.Models;
using Task07.Core.Interfaces;
using Task07.Application.Services;
using Task07.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// После builder.Services.AddControllers();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task07 API", Version = "v1" });
});

// Register your services
// Упростим регистрацию - используем только Core сервисы
builder.Services.AddScoped<Task07.Core.Interfaces.IItemService, Task07.Application.Services.ItemService>();
builder.Services.AddScoped<Task07.Core.Interfaces.IItemRepository, Task07.Infrastructure.Data.MockItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();