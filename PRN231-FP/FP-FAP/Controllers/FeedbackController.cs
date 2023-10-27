namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class FeedbackController : UserInfoController
{
}