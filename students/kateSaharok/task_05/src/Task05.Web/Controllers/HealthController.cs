using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task05.Web.Controllers;

[Authorize(Roles = "Admin")]
public class HealthController : Controller
{
    public IActionResult Check()
    {
        return View();
    }
}