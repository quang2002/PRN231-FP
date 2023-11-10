namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using FP_FAP.DTO;
using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/group")]
public class GroupController : UserInfoController
{
    [HttpGet]
    public async Task<IActionResult> GetGroups(
        [FromQuery] string? semester,
        CancellationToken   cancellationToken
    )
    {
        var groups = await this.GroupRepository.GetGroupsAsync(semester, cancellationToken);
        return this.Ok(groups);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGroupById(
        int               id,
        CancellationToken cancellationToken
    )
    {
        var group = await this.GroupRepository.GetGroupByIdAsync(id, cancellationToken);

        if (group == null)
        {
            return this.NotFound("Group not found");
        }

        foreach (var enroll in group.Enrolls)
        {
            enroll.Student.Enrolls   = null!;
            enroll.Student.Feedbacks = null!;

            enroll.Group = null!;
        }

        foreach (var feedback in group.Feedbacks)
        {
            feedback.Student.Feedbacks = null!;
            feedback.Student.Enrolls   = null!;
            
            feedback.Group = null!;
        }
        
        group.Teacher.Enrolls   = null!;
        group.Teacher.Feedbacks = null!;
        group.Subject.Groups = null!;

        return this.Ok(group);
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
            SubjectId = group.SubjectId,
            TeacherId = group.TeacherId,
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
        var authorizedId = this.UserInfo!.Id;
        if (this.UserInfo?.Role == Roles.Student && request.Students.Any(e => e != authorizedId))
        {
            return this.Unauthorized("You can only enroll yourself");
        }

        var result = await this.GroupRepository.EnrollStudentAsync(
            request.GroupId,
            request.Students,
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
        var authorizedId = this.UserInfo!.Id;
        if (this.UserInfo?.Role == Roles.Student && request.Students.Any(e => e != authorizedId))
        {
            return this.Unauthorized("You can only unenroll yourself");
        }

        var result = await this.GroupRepository.UnenrollStudentAsync(
            request.GroupId,
            request.Students,
            cancellationToken
        );

        if (!result)
        {
            return this.BadRequest("There are some students could not be unenroll");
        }

        return this.Ok("Student unenroll successfully");
    }

    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGroup(
        [FromBody] int[]  id,
        CancellationToken cancellationToken
    )
    {
        var result = await this.GroupRepository.DeleteGroupAsync(
            id,
            cancellationToken
        );

        if (!result)
        {
            return this.BadRequest("Group could not be delete");
        }

        return this.Ok("Group deleted successfully");
    }

    #region Inject

    public GroupController(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        this.GroupRepository = groupRepository;
        this.UserRepository  = userRepository;
    }

    private IGroupRepository GroupRepository { get; }
    private IUserRepository  UserRepository  { get; }

    #endregion
}