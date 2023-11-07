namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class EnrollStudentRequest
{
    [Required]
    [JsonPropertyName("group-id")]
    public string GroupId { get; set; } = null!;

    [Required]
    [JsonPropertyName("students")]
    public string[] Students { get; set; } = null!;
}