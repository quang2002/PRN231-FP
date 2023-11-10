namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/feedback")]
public class FeedbackController : UserInfoController
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFeedbackAsync(int id, CancellationToken cancellationToken)
    {
        var feedback = await this.FeedbackRepository.GetFeedbackAsync(id, cancellationToken);
        if (feedback is null)
        {
            return this.NotFound();
        }

        feedback.Student.Feedbacks       = null!;
        feedback.Student.Enrolls         = null!;
        feedback.Group.Subject.Groups    = null!;
        feedback.Group.Feedbacks         = null!;
        feedback.Group.Teacher.Feedbacks = null!;
        feedback.Group.Teacher.Enrolls   = null!;

        return this.Ok(feedback);
    }

    [HttpPost("assign")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignFeedbackAsync([FromBody] AssignFeedbackRequest request,
                                                         CancellationToken                cancellationToken)
    {
        try
        {
            await this.FeedbackRepository.UpdateOrAddAsync(
                request.Students.Select(studentId => new Feedback
                {
                    GroupId   = request.GroupId,
                    StudentId = studentId,
                }),
                cancellationToken
            );
            return this.Ok(true);
        }
        catch
        {
            return this.BadRequest(false);
        }
    }

    [HttpPost]
    public async Task<IActionResult> DoFeedbackAsync([FromBody] Feedback feedback,
                                                     CancellationToken   cancellationToken)
    {
        try
        {
            feedback.IsDone = true;
            
            var result = await this.FeedbackRepository.UpdateOrAddAsync(
                new[] { feedback },
                cancellationToken
            );

            return this.Ok(result);
        }
        catch
        {
            return this.BadRequest(false);
        }
    }

    public record AssignFeedbackRequest(
        int   GroupId,
        int[] Students
    );

    #region Inject

    private IFeedbackRepository FeedbackRepository { get; }

    public FeedbackController(IFeedbackRepository feedbackRepository)
    {
        this.FeedbackRepository = feedbackRepository;
    }

    #endregion
}