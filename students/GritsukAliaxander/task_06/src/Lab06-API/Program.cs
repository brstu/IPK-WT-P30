using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Настройка URL
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

builder.Services.AddControllers();
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API v1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Products API v2", Version = "v2" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Запуск приложения
app.Run();