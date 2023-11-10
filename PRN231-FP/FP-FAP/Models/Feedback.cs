namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Feedback")]
public class Feedback
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int StudentId { get; set; }

    [Required]
    [ForeignKey(nameof(Models.Group))]
    public int GroupId { get; set; }

    public bool IsDone { get; set; }

    public int Punctuality { get; set; }

    public int Skill { get; set; }

    public int Adequately { get; set; }

    public int Support { get; set; }

    public int Response { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    public virtual User Student { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}