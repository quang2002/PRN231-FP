namespace FP_FAP.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return this.Ok("Test");
    }
    
    [HttpGet("auth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return this.Ok($"TestAuth {this.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value}");
    }
}