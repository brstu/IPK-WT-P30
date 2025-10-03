using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task05.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Please fill in all fields";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Email, Password, false, false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} logged in successfully", Email);
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Invalid email or password";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", Email);
            ErrorMessage = "An error occurred during login";
            return Page();
        }
    }
}