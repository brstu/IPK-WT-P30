using Microsoft.EntityFrameworkCore;
using Task05.Domain.Interfaces;
using Task05.Infrastructure.Services;
using Task05.Infrastructure.Data;
using Task05.Domain.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity with roles support
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register our custom services
builder.Services.AddScoped<IDateTimeService, DateTimeService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Seed database with admin role and user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        Console.WriteLine("=== SEED DATA START ===");

        // Create Admin role
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            Console.WriteLine("✅ Admin role created");
        }
        else
        {
            Console.WriteLine("✅ Admin role already exists");
        }

        // Create admin user
        var adminEmail = "admin@test.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User"
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("✅ Admin user created successfully");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine("✅ Admin user already exists");
            
            // Check if user has Admin role
            var isInRole = await userManager.IsInRoleAsync(adminUser, "Admin");
            Console.WriteLine($"✅ User is in Admin role: {isInRole}");
            
            // If not in role, add to role
            if (!isInRole)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("✅ Added Admin role to existing user");
            }
        }

        Console.WriteLine("=== SEED DATA COMPLETE ===");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Debug: Show all routes
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/debug-routes")
    {
        await context.Response.WriteAsync("Available routes:\n");
        await context.Response.WriteAsync("- / (Home/Index)\n");
        await context.Response.WriteAsync("- /Account/Login\n");
        await context.Response.WriteAsync("- /Account/Register\n");
        await context.Response.WriteAsync("- /Account/Logout\n");
        await context.Response.WriteAsync("- /Files/Index\n");
        await context.Response.WriteAsync("- /Health/Check\n");
        context.Response.ContentType = "text/plain";
        return;
    }
    await next();
});
app.Run();