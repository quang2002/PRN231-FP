namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;

public class CreateGroupRequest
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("subject-id")]
    public string SubjectId { get; set; } = null!;

    [Required]
    [JsonPropertyName("teacher-id")]
    public string TeacherId { get; set; } = null!;

    [Required]
    [JsonPropertyName("semester")]
    public string Semester { get; set; } = null!;

    [Required]
    [JsonPropertyName("students")]
    public string[] Students { get; set; } = null!;
}