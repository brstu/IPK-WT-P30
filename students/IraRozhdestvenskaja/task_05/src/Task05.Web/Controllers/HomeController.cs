using Microsoft.AspNetCore.Mvc;

namespace Task05.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Content(@"
<html>
<head>
    <title>Task05 - Main Page</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <h1>Task05 - Main Page</h1>
        <div class='alert alert-success'>
            <h4>Application is running! ✅</h4>
        </div>
        
        <h5>Available Pages:</h5>
        <ul>
            <li><a href='/File/Upload'>File Upload</a></li>
            <li><a href='/File/Admin'>Admin Panel</a></li>
            <li><a href='/Account/Login'>Login</a></li>
            <li><a href='/Account/Register'>Register</a></li>
            <li><a href='/test'>Test Route</a></li>
            <li><a href='/test2'>Test Page</a></li>
        </ul>

        <div class='mt-4'>
            <h5>Test Credentials:</h5>
            <div class='alert alert-info'>
                <strong>Admin:</strong> admin@example.com / Admin123!
            </div>
        </div>
    </div>
</body>
</html>", "text/html");
    }
}