using Microsoft.AspNetCore.Mvc;
using Task05.Domain.Interfaces;

namespace Task05.Web.Controllers;

public class HomeController : Controller
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IDateTimeService dateTimeService, ILogger<HomeController> logger)
    {
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["LocalTime"] = _dateTimeService.Now.ToString("dd.MM.yyyy HH:mm:ss");
        ViewData["UtcTime"] = _dateTimeService.UtcNow.ToString("dd.MM.yyyy HH:mm:ss");
        
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}