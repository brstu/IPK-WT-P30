using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Test endpoint is working! " + DateTime.Now);
    }
    
    [HttpGet("hello")]
    public IActionResult Hello()
    {
        return Ok(new { message = "Hello World!", timestamp = DateTime.Now });
    }
}
