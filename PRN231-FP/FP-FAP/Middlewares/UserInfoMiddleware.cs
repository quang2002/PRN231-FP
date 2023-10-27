namespace FP_FAP.Middlewares;

using System.Security.Claims;
using FP_FAP.Models;

public class UserInfoMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers["Authorization"].FirstOrDefault() is not null)
        {
            await this.AttachUserToContext(context);
        }

        await this.Next(context);
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

        context.Items["User"] = await this.UserCollection.GetByEmailAsync(email);
    }

    #region Inject

    public UserInfoMiddleware(RequestDelegate next, UserCollection userCollection)
    {
        this.Next           = next;
        this.UserCollection = userCollection;
    }

    private RequestDelegate Next           { get; }
    private UserCollection  UserCollection { get; }

    #endregion
}