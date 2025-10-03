using Microsoft.AspNetCore.Mvc;
using Task05.Application.Interfaces;

namespace Task05.Web.Controllers;

public class DiagnosticController : Controller
{
    private readonly IFileService _fileService;

    public DiagnosticController(IFileService fileService)
    {
        _fileService = fileService;
    }

    public IActionResult Index()
    {
        var diagnostics = new
        {
            FileServiceRegistered = _fileService != null,
            FileServiceType = _fileService?.GetType().Name,
            CurrentTime = DateTime.Now,
            Routes = new[] 
            {
                "/File/Upload",
                "/File/Admin",
                "/Account/Login", 
                "/Account/Register",
                "/Diagnostic"
            }
        };

        ViewBag.Diagnostics = diagnostics;
        return View();
    }
}