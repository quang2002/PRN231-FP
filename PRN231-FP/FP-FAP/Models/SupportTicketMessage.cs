namespace FP_FAP.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(SupportTicketMessage))]
public class SupportTicketMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Models.SupportTicket))]
    public int SupportTicketId { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int SenderId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [MaxLength(100)]
    public string? AttachmentName { get; set; }

    public byte[] Attachment { get; set; } = null!;

    public virtual SupportTicket SupportTicket { get; set; } = null!;
    public virtual User          Sender        { get; set; } = null!;
}