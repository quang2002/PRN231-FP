namespace FP_FAP.Controllers.Base;

using FP_FAP.Models;
using Microsoft.AspNetCore.Mvc;

public abstract class UserInfoController : ControllerBase
{
    protected virtual User? UserInfo
    {
        get
        {
            if (!this.HttpContext.Items.TryGetValue("User", out var user))
            {
                return null;
            }

            return user as User;
        }
    }
}