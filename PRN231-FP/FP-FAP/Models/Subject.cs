namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Subject))]
public class Subject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    public virtual List<Group> Groups { get; set; } = new();
}