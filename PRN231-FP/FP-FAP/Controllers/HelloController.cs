namespace FP_FAP.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/hello")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public string Index()
    {
        return "[PRN231-FP] - FAP";
    }
}