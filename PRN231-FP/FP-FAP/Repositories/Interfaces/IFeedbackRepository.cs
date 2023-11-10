namespace FP_FAP.Repositories.Interfaces;

using FP_FAP.Models;

public interface IFeedbackRepository
{
    Task<bool> UpdateOrAddAsync(IEnumerable<Feedback> feedbacks,
                                CancellationToken     cancellationToken = default
    );

    Task<Feedback?> GetFeedbackAsync(int               id,
                                     CancellationToken cancellationToken = default
    );
}