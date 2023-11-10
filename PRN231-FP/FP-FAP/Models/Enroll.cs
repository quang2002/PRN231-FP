namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Enroll))]
public class Enroll
{
    [Required]
    [ForeignKey(nameof(User))]
    public int StudentId { get; set; }

    [Required]
    [ForeignKey(nameof(Models.Group))]
    public int GroupId { get; set; }

    public virtual User  Student { get; set; } = null!;
    public virtual Group Group   { get; set; } = null!;
}