using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task05.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Task05.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} logged in", email);
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.Error = "Неверный email или пароль";
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ViewBag.Error = "Произошла ошибка при входе";
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string confirmPassword)
    {
        try
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Пароли не совпадают";
                return View();
            }

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} registered", email);
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            ViewBag.Error = "Произошла ошибка при регистрации";
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}