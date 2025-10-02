using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task05.Application.Interfaces;
using Task05.Domain.Entities;
using Task05.Domain.Interfaces;
using Task05.Infrastructure.Data;
using Task05.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка Identity [citation:7][citation:10]
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Регистрация сервисов [citation:2][citation:5][citation:8]
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserInitializerService>();

builder.Services.AddHostedService<UserInitializerService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Конфигурация пайплайна HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Для обслуживания файлов из wwwroot

app.UseRouting();

app.UseAuthentication(); // Аутентификация должна быть до авторизации [citation:7]
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();