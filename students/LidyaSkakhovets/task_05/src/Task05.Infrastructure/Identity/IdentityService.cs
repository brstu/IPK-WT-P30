using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task05.Application.Common.Interfaces;
using Task05.Infrastructure.Data;

namespace Task05.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        return user.UserName!;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        return await _userManager.IsInRoleAsync(user, role);
    }

    public Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        // Simplified authorization
        return Task.FromResult(true);
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser { UserName = userName, Email = userName };
        var result = await _userManager.CreateAsync(user, password);
        
        if (result.Succeeded)
            return (Result.Success(), user.Id);
        
        return (Result.Failure(result.Errors.Select(e => e.Description)), string.Empty);
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        var result = await _userManager.DeleteAsync(user);
        
        if (result.Succeeded)
            return Result.Success();
        
        return Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task CreateRoleAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    public async Task AddToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        await _userManager.AddToRoleAsync(user, roleName);
    }
}

public class ApplicationUser : IdentityUser { }