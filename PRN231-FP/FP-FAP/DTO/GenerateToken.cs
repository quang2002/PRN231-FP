namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class GenerateTokenRequest
{
    [Required]
    [JsonPropertyName("google-access-token")]
    public string GoogleAccessToken { get; set; } = null!;
}

public class OperatorGenerateTokenRequest
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;

    [Required]
    [JsonPropertyName("secret-key")]
    public string SecretKey { get; set; } = null!;
}

public class GenerateTokenResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}