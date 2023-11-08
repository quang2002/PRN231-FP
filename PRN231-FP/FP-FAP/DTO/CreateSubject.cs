namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class CreateSubject
{
    [Required]
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}