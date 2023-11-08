namespace FP_FAP.Repositories.Interfaces;

using FP_FAP.Models;

public interface ISupportTicketRepository
{
    Task<bool> CreateSupportTicketAsync(SupportTicket     supportTicket,
                                        CancellationToken cancellationToken = default
    );

    Task<bool> CloseSupportTicketAsync(int               supportTicketId,
                                       CancellationToken cancellationToken = default
    );

    Task<bool> ReopenSupportTicketAsync(int               supportTicketId,
                                        CancellationToken cancellationToken = default
    );

    Task<bool> CreateSupportTicketMessageAsync(SupportTicketMessage supportTicketMessage,
                                               CancellationToken    cancellationToken = default
    );

    Task<SupportTicket?> GetSupportTicketAsync(int               supportTicketId,
                                               CancellationToken cancellationToken = default
    );

    Task<IEnumerable<SupportTicket>> GetRelatedSupportTicketsAsync(string            email,
                                                                   CancellationToken cancellationToken = default
    );
}