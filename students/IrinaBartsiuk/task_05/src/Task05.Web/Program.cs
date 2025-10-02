using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task05.Web.Data;
using Task05.Application.Interfaces;
using Task05.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Services
builder.Services.AddScoped<IFileService, FileService>();

// Razor Pages and MVC
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ВКЛЮЧАЕМ ПОДРОБНЫЕ ОШИБКИ
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

// Map MVC Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map minimal API для теста
app.MapGet("/test", () => "Test route works! ✅");
app.MapGet("/test2", () => Results.Text(@"
<html>
<body>
    <h1>Test Page</h1>
    <p>Minimal API works!</p>
    <a href='/File/Upload'>Go to File Upload</a>
</body>
</html>", "text/html"));

app.Run();