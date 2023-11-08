namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/support-tickets")]
public class SupportTicketController : UserInfoController
{
    [HttpGet]
    public async Task<IActionResult> GetTicketsAsync(CancellationToken cancellationToken)
    {
        var email   = this.UserInfo!.Email;
        var tickets = await this.SupportTicketRepository.GetRelatedSupportTicketsAsync(email, cancellationToken);
        return this.Ok(tickets);
    }

    [HttpGet("{ticketId:int}")]
    public async Task<IActionResult> GetTicketAsync(int ticketId, CancellationToken cancellationToken)
    {
        var ticket = await this.SupportTicketRepository.GetSupportTicketAsync(ticketId, cancellationToken);

        if (ticket is null)
        {
            return this.NotFound();
        }

        foreach (var message in ticket.Messages)
        {
            message.SupportTicket = null!;
        }

        return this.Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicketAsync([FromBody] CreateSupportTicketRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var ticket = new SupportTicket
            {
                IssuerId   = this.UserInfo!.Id,
                AssigneeId = request.AssigneeId,
                Title      = request.Title,
                Status     = SupportTicket.SupportTicketStatus.Open,
                CreatedAt  = DateTime.UtcNow,
                UpdatedAt  = DateTime.UtcNow,
                Messages = new List<SupportTicketMessage>
                {
                    new()
                    {
                        SenderId       = this.UserInfo!.Id,
                        Message        = request.Description,
                        CreatedAt      = DateTime.UtcNow,
                        Attachment     = request.Attachment,
                        AttachmentName = request.filename,
                    },
                },
            };

            await this.SupportTicketRepository.CreateSupportTicketAsync(ticket, cancellationToken);
            return this.Ok();
        }
        catch
        {
            return this.BadRequest();
        }
    }

    [HttpPost("{ticketId:int}")]
    public async Task<IActionResult> CreateTicketMessageAsync(int ticketId, [FromBody] CreateSupportTicketMessageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var message = new SupportTicketMessage
            {
                SupportTicketId = ticketId,
                SenderId        = this.UserInfo!.Id,
                Message         = request.Message,
                CreatedAt       = DateTime.UtcNow,
                Attachment      = request.Attachment,
                AttachmentName  = request.filename,
            };
            await this.SupportTicketRepository.CreateSupportTicketMessageAsync(message, cancellationToken);
            return this.Ok();
        }
        catch
        {
            return this.BadRequest();
        }
    }

    [HttpPut("{ticketId:int}/reopen")]
    public async Task<IActionResult> ReopenTicketAsync(int ticketId, CancellationToken cancellationToken)
    {
        try
        {
            await this.SupportTicketRepository.ReopenSupportTicketAsync(ticketId, cancellationToken);
            return this.Ok();
        }
        catch
        {
            return this.BadRequest();
        }
    }

    [HttpPut("{ticketId:int}/close")]
    public async Task<IActionResult> CloseTicketAsync(int ticketId, CancellationToken cancellationToken)
    {
        try
        {
            await this.SupportTicketRepository.CloseSupportTicketAsync(ticketId, cancellationToken);
            return this.Ok();
        }
        catch
        {
            return this.BadRequest();
        }
    }

    public record CreateSupportTicketRequest(
        string Title,
        string Description,
        int    AssigneeId,
        byte[] Attachment,
        string filename
    );

    public record CreateSupportTicketMessageRequest(
        string Message,
        byte[] Attachment,
        string filename
    );

    #region Inject

    private ISupportTicketRepository SupportTicketRepository { get; }

    public SupportTicketController(ISupportTicketRepository supportTicketRepository)
    {
        this.SupportTicketRepository = supportTicketRepository;
    }

    #endregion
}