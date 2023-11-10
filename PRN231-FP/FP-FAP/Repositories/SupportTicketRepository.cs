namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class SupportTicketRepository : ISupportTicketRepository
{
    public async Task<bool> CreateSupportTicketAsync(SupportTicket supportTicket, CancellationToken cancellationToken = default)
    {
        supportTicket.CreatedAt = DateTime.UtcNow;
        supportTicket.UpdatedAt = DateTime.UtcNow;

        var result = await this.DBContext.SupportTickets.AddAsync(supportTicket, cancellationToken);
        await this.DBContext.SaveChangesAsync(cancellationToken);
        return result.State == EntityState.Added;
    }

    public async Task<bool> CloseSupportTicketAsync(int supportTicketId, CancellationToken cancellationToken = default)
    {
        var supportTicket = await this.DBContext.SupportTickets.FindAsync(new object?[] { supportTicketId }, cancellationToken);

        if (supportTicket == null)
        {
            return false;
        }

        supportTicket.Status    = SupportTicket.SupportTicketStatus.Closed;
        supportTicket.UpdatedAt = DateTime.UtcNow;

        this.DBContext.SupportTickets.Update(supportTicket);

        return await this.DBContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> ReopenSupportTicketAsync(int supportTicketId, CancellationToken cancellationToken = default)
    {
        var supportTicket = await this.DBContext.SupportTickets.FindAsync(new object?[] { supportTicketId }, cancellationToken);

        if (supportTicket == null)
        {
            return false;
        }

        supportTicket.Status    = SupportTicket.SupportTicketStatus.Reopen;
        supportTicket.UpdatedAt = DateTime.UtcNow;

        this.DBContext.SupportTickets.Update(supportTicket);

        return await this.DBContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> CreateSupportTicketMessageAsync(SupportTicketMessage supportTicketMessage, CancellationToken cancellationToken = default)
    {
        var supportTicket = await this.DBContext.SupportTickets.FindAsync(new object?[] { supportTicketMessage.SupportTicketId }, cancellationToken);

        if (supportTicket == null)
        {
            return false;
        }

        if (supportTicket.Status == SupportTicket.SupportTicketStatus.Closed)
        {
            return false;
        }

        supportTicket.UpdatedAt = DateTime.UtcNow;

        this.DBContext.SupportTickets.Update(supportTicket);
        await this.DBContext.SupportTicketMessages.AddAsync(supportTicketMessage, cancellationToken);

        return await this.DBContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public Task<SupportTicket?> GetSupportTicketAsync(int supportTicketId, CancellationToken cancellationToken = default)
    {
        return this.DBContext.SupportTickets
                   .Include(e => e.Issuer)
                   .Include(e => e.Assignee)
                   .Include(e => e.Messages)
                   .FirstOrDefaultAsync(e => e.Id == supportTicketId, cancellationToken);
    }

    public async Task<IEnumerable<SupportTicket>> GetRelatedSupportTicketsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await this.DBContext.SupportTickets
                         .Include(e => e.Issuer)
                         .Include(e => e.Assignee)
                         .Where(e => e.Issuer.Email == email || e.Assignee.Email == email)
                         .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SupportTicketMessage>> GetSupportTicketMessagesAsync(int supportTicketId, CancellationToken cancellationToken = default)
    {
        return await this.DBContext.SupportTicketMessages
                         .Include(e => e.SupportTicket)
                         .Include(e => e.Sender)
                         .Where(e => e.SupportTicketId == supportTicketId)
                         .ToListAsync(cancellationToken);
    }

    #region Inject

    private ProjectDbContext DBContext { get; }

    public SupportTicketRepository(ProjectDbContext dbContext)
    {
        this.DBContext = dbContext;
    }

    #endregion
}