using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task05.Domain.Entities;
using Task05.Domain.Interfaces;
using Task05.Infrastructure.Data;
using Task05.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы в контейнер ДО Build()
builder.Services.AddSingleton<IClock, SystemClock>();

// Настройка базы данных
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Настройка Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Добавляем поддержку MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Добавляем аутентификацию
app.UseAuthorization();  // Добавляем авторизацию

// Настраиваем маршруты для MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Маршрут для Identity pages (регистрация/логин)
app.MapRazorPages();

app.Run();