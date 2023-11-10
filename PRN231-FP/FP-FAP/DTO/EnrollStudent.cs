namespace FP_FAP.DTO;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class EnrollStudentRequest
{
    [Required]
    public int GroupId { get; set; }

    [Required]
    public int[] Students { get; set; } = null!;
}