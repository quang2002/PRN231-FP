namespace FP_FAP.Controllers;

using FP_FAP.Controllers.Base;
using FP_FAP.DTO;
using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/subject")]
public class SubjectController : UserInfoController
{
    [HttpGet]
    public async Task<IActionResult> GetAllSubjects(CancellationToken cancellationToken)
    {
        var subjects = await this.SubjectRepository.GetAllSubjectsAsync(cancellationToken);
        return this.Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id, CancellationToken cancellationToken)
    {
        var subject = await this.SubjectRepository.GetSubjectByIdAsync(id, cancellationToken);
        return this.Ok(subject);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubject subject, CancellationToken cancellationToken)
    {
        try
        {
            var createdSubject = await this.SubjectRepository.CreateSubjectAsync(new Subject
            {
                Code = subject.Code,
                Name = subject.Name,
            }, cancellationToken);

            return this.Ok(createdSubject);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    #region Inject

    private ISubjectRepository SubjectRepository { get; }

    public SubjectController(ISubjectRepository subjectRepository)
    {
        this.SubjectRepository = subjectRepository;
    }

    #endregion
}