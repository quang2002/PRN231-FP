namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class FeedbackRepository : IFeedbackRepository
{
    public async Task<bool> UpdateOrAddAsync(IEnumerable<Feedback> feedbacks, CancellationToken cancellationToken = default)
    {
        foreach (var feedback in feedbacks)
        {
            var existingFeedback = await this.DBContext.Feedbacks
                                             .FirstOrDefaultAsync(
                                                 f => (f.GroupId == feedback.GroupId && f.StudentId == feedback.StudentId) || f.Id == feedback.Id,
                                                 cancellationToken
                                             );

            if (existingFeedback != null)
            {
                existingFeedback.IsDone = feedback.IsDone;

                existingFeedback.Adequately  = feedback.Adequately;
                existingFeedback.Punctuality = feedback.Punctuality;
                existingFeedback.Response    = feedback.Response;
                existingFeedback.Skill       = feedback.Skill;
                existingFeedback.Support     = feedback.Support;

                existingFeedback.Comment = feedback.Comment;
            }
            else
            {
                await this.DBContext.Feedbacks.AddAsync(feedback, cancellationToken);
            }
        }

        return await this.DBContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Feedback?> GetFeedbackAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.DBContext.Feedbacks
                         .Include(e => e.Group.Subject)
                         .Include(e => e.Group.Teacher)
                         .Include(e => e.Student)
                         .FirstOrDefaultAsync(
                             f => f.Id == id,
                             cancellationToken
                         );
    }

    #region Inject

    private ProjectDbContext DBContext { get; }

    public FeedbackRepository(ProjectDbContext dbContext)
    {
        this.DBContext = dbContext;
    }

    #endregion
}