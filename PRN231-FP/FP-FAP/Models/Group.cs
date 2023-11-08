namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Group))]
public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string Name { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Models.Subject))]
    public int SubjectId { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int TeacherId { get; set; }

    [Required]
    [MaxLength(10)]
    public string Semester { get; set; } = null!;

    public virtual List<Feedback> Feedbacks { get; set; } = new();
    public virtual List<Enroll>   Enrolls   { get; set; } = new();
    public virtual Subject        Subject   { get; set; } = null!;
    public virtual User           Teacher   { get; set; } = null!;
}