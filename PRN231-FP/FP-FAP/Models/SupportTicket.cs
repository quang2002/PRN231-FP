namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(SupportTicket))]
public class SupportTicket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int IssuerId { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int AssigneeId { get; set; }

    [Required]
    [MaxLength(10)]
    public string Status { get; set; } = SupportTicketStatus.Open;

    public virtual User Issuer   { get; set; } = null!;
    public virtual User Assignee { get; set; } = null!;

    public virtual List<SupportTicketMessage> Messages { get; set; } = new();

    public class SupportTicketStatus
    {
        public const string Open   = "open";
        public const string Closed = "closed";
        public const string Reopen = "reopen";
    }
}