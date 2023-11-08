namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/user")]
public class UserController : UserInfoController
{
    [HttpGet("all")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllUserAsync(CancellationToken cancellationToken = default)
    {
        var users = await this.UserRepository.GetAllAsync(cancellationToken);
        return this.Ok(users);
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await this.UserRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            return this.NotFound();
        }

        return this.Ok(user);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var email = this.UserInfo?.Email;

        if (string.IsNullOrEmpty(email))
        {
            return this.BadRequest("No email claim found in token.");
        }

        var user = await this.UserRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return this.NotFound();
        }

        return this.Ok(user);
    }

    [HttpGet("feedbacks")]
    public async Task<IActionResult> GetUserFeedbacksAsync(CancellationToken cancellationToken = default)
    {
        var email = this.UserInfo?.Email;

        if (string.IsNullOrEmpty(email))
        {
            return this.BadRequest("No email claim found in token.");
        }

        var feedbacks = (await this.UserRepository.GetUserFeedbacksAsync(email, cancellationToken)).ToArray();

        if (!feedbacks.Any())
        {
            return this.NotFound();
        }

        foreach (var feedback in feedbacks)
        {
            feedback.Student.Enrolls      = null!;
            feedback.Student.Feedbacks    = null!;
            feedback.Group.Feedbacks      = null!;
            feedback.Group.Subject.Groups = null!;
        }

        return this.Ok(feedbacks);
    }

    #region Inject

    public UserController(IUserRepository userRepository)
    {
        this.UserRepository = userRepository;
    }

    private IUserRepository UserRepository { get; }

    #endregion
}