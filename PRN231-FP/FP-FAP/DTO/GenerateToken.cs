namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class GenerateTokenRequest
{
    [Required]
    [JsonPropertyName("google-access-token")]
    public string GoogleAccessToken { get; set; } = null!;
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