namespace FP_Auth.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SharedService.WebAPI;

[Route("/api/auth")]
public class AuthController : ControllerBase
{
    public record VerifyTokenResponse : BaseResponseModel
    {
        public bool IsValid { get; set; }
    }

    [HttpPost("verify-token")]
    public async Task<VerifyTokenResponse> VerifyToken(
        [FromHeader(Name = "Authorization")] string token,
        CancellationToken                           cancellationToken
    )
    {
        const string prefix = "Bearer ";

        if (!token.StartsWith(prefix))
        {
            return new VerifyTokenResponse
            {
                IsValid = false,
                Message = $"{token} is not a valid token, it should start with Bearer",
            };
        }

        var tokenValue = token[prefix.Length..];

        var result = await this.HttpContext.AuthenticateAsync();

        return new VerifyTokenResponse
        {
            IsValid = true,
            Message = $"{token} is valid",
        };
    }
}