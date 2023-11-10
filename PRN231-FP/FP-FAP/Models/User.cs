namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Roles
{
    public const string Student = "student";
    public const string Teacher = "teacher";
    public const string Admin   = "admin";
}

[Table(nameof(User))]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string Role { get; set; } = null!;

    public virtual List<Enroll>   Enrolls   { get; set; } = new();
    public virtual List<Feedback> Feedbacks { get; set; } = new();
}