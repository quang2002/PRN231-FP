namespace FP_FAP.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FP_FAP.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Authorize]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private TimeSpan TokenLifetime { get; } = TimeSpan.FromMinutes(30);

    #region Inject

    private IConfiguration Configuration { get; }

    public AuthController(
        IConfiguration configuration
    )
    {
        this.Configuration = configuration;
    }

    #endregion

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateToken(
        [FromBody] GenerateTokenRequest request,
        CancellationToken               cancellationToken
    )
    {
        try
        {
            var tokenInfo = await this.InternalGoogleTokenInfoAsync(request.GoogleAccessToken, cancellationToken);
            var token     = this.InternalGenerateToken(tokenInfo.Email, "Admin", DateTime.Now.Add(this.TokenLifetime));

            return this.Ok(new GenerateTokenResponse
            {
                Success = true,
                Message = "Generate token successfully",
                Token   = token,
            });
        }
        catch (Exception e)
        {
            return this.BadRequest(new GenerateTokenResponse
            {
                Success = false,
                Message = e.Message,
            });
        }
    }

    private async Task<GoogleTokenInfo> InternalGoogleTokenInfoAsync(string googleAccessToken, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(
            $"https://oauth2.googleapis.com/tokeninfo?access_token={googleAccessToken}",
            cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Invalid google api token");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var tokenInfo = JsonSerializer.Deserialize<GoogleTokenInfo>(content);

        if (tokenInfo is null)
        {
            throw new Exception("Invalid google api token");
        }

        return tokenInfo;
    }

    private string InternalGenerateToken(string email, string role, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key          = Encoding.UTF8.GetBytes(this.Configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, email),
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private class GoogleTokenInfo
    {
        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("verified_email")]
        public string VerifiedEmail { get; set; } = null!;

        [JsonPropertyName("access_type")]
        public string AccessType { get; set; } = null!;
    }
}