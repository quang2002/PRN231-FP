namespace FP_FAP.Middlewares;

using System.Security.Claims;
using FP_FAP.Repositories.Interfaces;

public class UserInfoMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers["Authorization"].FirstOrDefault() is not null)
        {
            await this.AttachUserToContext(context);
        }

        await next(context);
    }

    private async Task AttachUserToContext(HttpContext context)
    {
        var user = context.User;

        if (user is not { Identity.IsAuthenticated: true })
        {
            return;
        }

        var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return;
        }

        context.Items["User"] = await this.UserRepository.GetByEmailAsync(email);
    }

    #region Inject

    public UserInfoMiddleware(IUserRepository userRepository)
    {
        this.UserRepository = userRepository;
    }

    private IUserRepository UserRepository { get; }

    #endregion
}