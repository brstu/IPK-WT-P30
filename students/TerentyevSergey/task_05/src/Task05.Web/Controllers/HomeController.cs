using Microsoft.AspNetCore.Mvc;
using Task05.Application.Common.Interfaces;

namespace Task05.Web.Controllers;

public class HomeController : Controller
{
    private readonly IDateTime _dateTime;

    public HomeController(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public IActionResult Index()
    {
        ViewBag.CurrentTime = _dateTime.Now;
        return View();
    }
}