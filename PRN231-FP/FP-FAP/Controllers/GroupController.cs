namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using FP_FAP.DTO;
using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

[Authorize]
[Route("api/group")]
public class GroupController : UserInfoController
{
    [HttpGet("{semester}")]
    public async Task<IActionResult> GetGroups(
        string?           semester,
        CancellationToken cancellationToken
    )
    {
        var groups = await this.GroupRepository.GetGroupsAsync(semester, cancellationToken);
        return this.Ok(groups);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateGroup(
        [FromBody] CreateGroupRequest group,
        CancellationToken             cancellationToken
    )
    {
        var result = await this.GroupRepository.CreateGroupAsync(new Group
        {
            Name      = group.Name,
            Semester  = group.Semester,
            SubjectId = new ObjectId(group.SubjectId),
            TeacherId = new ObjectId(group.TeacherId),
            Students  = group.Students.Select(e => new ObjectId(e)).ToArray(),
        }, cancellationToken);

        if (!result)
        {
            return this.BadRequest("Group could not be create");
        }

        return this.Ok("Group created successfully");
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> EnrollStudent(
        [FromBody] EnrollStudentRequest request,
        CancellationToken               cancellationToken
    )
    {
        var authorizedId = this.UserInfo!.Id.ToString();
        if (this.UserInfo?.Role == Roles.Student && request.Students.Any(e => e != authorizedId))
        {
            return this.Unauthorized("You can only enroll yourself");
        }

        var result = await this.GroupRepository.EnrollStudentAsync(
            new ObjectId(request.GroupId),
            request.Students.Select(e => new ObjectId(e)).ToArray(),
            cancellationToken
        );

        if (!result)
        {
            return this.BadRequest("There are some students could not be enroll");
        }

        return this.Ok("Student enrolled successfully");
    }

    [HttpPost("unenroll")]
    public async Task<IActionResult> UnenrollStudent(
        [FromBody] EnrollStudentRequest request,
        CancellationToken               cancellationToken
    )
    {
        var authorizedId = this.UserInfo!.Id.ToString();
        if (this.UserInfo?.Role == Roles.Student && request.Students.Any(e => e != authorizedId))
        {
            return this.Unauthorized("You can only unenroll yourself");
        }

        var result = await this.GroupRepository.UnenrollStudentAsync(
            new ObjectId(request.GroupId),
            request.Students.Select(e => new ObjectId(e)).ToArray(),
            cancellationToken
        );

        if (!result)
        {
            return this.BadRequest("There are some students could not be unenroll");
        }

        return this.Ok("Student unenroll successfully");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGroup(
        string            id,
        CancellationToken cancellationToken
    )
    {
        var result = await this.GroupRepository.DeleteGroupAsync(
            new ObjectId(id),
            cancellationToken
        );

        if (!result)
        {
            return this.BadRequest("Group could not be delete");
        }

        return this.Ok("Group deleted successfully");
    }

    #region Inject

    public GroupController(IGroupRepository groupRepository)
    {
        this.GroupRepository = groupRepository;
    }

    private IGroupRepository GroupRepository { get; }

    #endregion
}