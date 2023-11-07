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
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;

    [Required]
    [JsonPropertyName("secret-key")]
    public string SecretKey { get; set; } = null!;
}