namespace FP_FAP.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FP_FAP.DTO;
using FP_FAP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Authorize]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private TimeSpan TokenLifetime { get; } = TimeSpan.FromDays(1);

    #region Inject

    private IConfiguration Configuration  { get; }
    private UserCollection UserCollection { get; }

    public AuthController(
        IConfiguration configuration,
        UserCollection userCollection
    )
    {
        this.Configuration  = configuration;
        this.UserCollection = userCollection;
    }

    #endregion

    [HttpGet("generate-token")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateToken(
        [FromBody] GenerateTokenRequest request,
        CancellationToken               cancellationToken
    )
    {
        try
        {
            var tokenInfo = await this.InternalGoogleTokenInfoAsync(request.GoogleAccessToken, cancellationToken);

            this.InternalBusinessCheck(tokenInfo);

            var user = await this.UserCollection.GetOrCreateByEmailAsync(tokenInfo.Email, cancellationToken);

            var token = this.InternalGenerateToken(user, DateTime.Now.Add(this.TokenLifetime));

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

    private void InternalBusinessCheck(GoogleTokenInfo tokenInfo)
    {
        if (!tokenInfo.Email.EndsWith("@fpt.edu.vn"))
        {
            throw new Exception("Only fpt email is allowed");
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

    private string InternalGenerateToken(User user, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key          = Encoding.UTF8.GetBytes(this.Configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
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