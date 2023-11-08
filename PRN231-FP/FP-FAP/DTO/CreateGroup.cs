namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class CreateGroupRequest
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("subjectId")]
    public int SubjectId { get; set; }

    [Required]
    [JsonPropertyName("teacherId")]
    public int TeacherId { get; set; }

    [Required]
    [JsonPropertyName("semester")]
    public string Semester { get; set; } = null!;
}