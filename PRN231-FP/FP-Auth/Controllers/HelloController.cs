using Microsoft.AspNetCore.Mvc;

namespace FP_Auth.Controllers;

[ApiController]
[Route("api/hello")]
public class HelloController : Controller
{
    [HttpGet]
    public string Index()
    {
        return "[PRN231-FP] - Auth";
    }
}