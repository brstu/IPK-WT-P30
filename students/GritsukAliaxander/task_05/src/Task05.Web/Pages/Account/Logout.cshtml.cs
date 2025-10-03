using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task05.Web.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userName = User.Identity?.Name ?? "Unknown";
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User {UserName} logged out", userName);
        return RedirectToPage("/Index");
    }
}