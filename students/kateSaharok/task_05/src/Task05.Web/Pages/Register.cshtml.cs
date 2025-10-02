using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task05.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Username, Email = Email };
                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    // Добавляем роль User по умолчанию
                    await _userManager.AddToRoleAsync(user, "User");
                    Message = "Регистрация успешна! Теперь вы можете войти.";
                    return RedirectToPage("/Login");
                }
                else
                {
                    Message = "Ошибка регистрации: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }

            return Page();
        }
    }
}