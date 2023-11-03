namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/feedback")]
public class FeedbackController : UserInfoController
{
}