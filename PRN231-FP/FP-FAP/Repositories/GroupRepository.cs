namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class GroupRepository : IGroupRepository
{
    public async Task<IEnumerable<Group>> GetGroupsAsync(string? semester, CancellationToken cancellationToken = default)
    {
        var result = this.DBContext.Groups.AsQueryable();

        if (!string.IsNullOrWhiteSpace(semester))
        {
            semester = semester.ToLower();
            result   = result.Where(x => x.Semester.ToLower() == semester);
        }

        var list = await result
                         .Include(e => e.Teacher)
                         .Include(e => e.Subject)
                         .ToListAsync(cancellationToken);

        foreach (var group in list)
        {
            group.Teacher.Enrolls = null!;
            group.Subject.Groups  = null!;
        }

        return list;
    }

    public Task<Group?> GetGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return this.DBContext.Groups
                   .Include(e => e.Enrolls)
                   .ThenInclude(e => e.Student)
                   .Include(e => e.Teacher)
                   .Include(e => e.Subject)
                   .Include(e => e.Feedbacks)
                   .ThenInclude(e => e.Student)
                   .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> CreateGroupAsync(Group group, CancellationToken cancellationToken = default)
    {
        try
        {
            await this.DBContext.Groups.AddAsync(group, cancellationToken);
            await this.DBContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EnrollStudentAsync(int groupId, IEnumerable<int> students, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await this.DBContext
                                  .Groups
                                  .Include(e => e.Enrolls)
                                  .FirstOrDefaultAsync(e => e.Id == groupId, cancellationToken);

            if (group is null)
            {
                return false;
            }

            group.Enrolls.AddRange(students.Select(studentId => new Enroll
            {
                GroupId   = groupId,
                StudentId = studentId,
            }));

            this.DBContext.Groups.Update(group);
            await this.DBContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UnenrollStudentAsync(int groupId, IEnumerable<int> students, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await this.DBContext
                                  .Groups
                                  .Include(e => e.Enrolls)
                                  .FirstOrDefaultAsync(e => e.Id == groupId, cancellationToken);

            if (group is null)
            {
                return false;
            }

            group.Enrolls.RemoveAll(enroll => students.Any(studentId => studentId == enroll.StudentId && enroll.GroupId == groupId));

            this.DBContext.Groups.Update(group);
            await this.DBContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteGroupAsync(int[] groupId, CancellationToken cancellationToken = default)
    {
        var groups = await this.DBContext.Groups.Where(x => groupId.Contains(x.Id)).ToListAsync(cancellationToken);

        if (!groups.Any())
        {
            return false;
        }

        this.DBContext.Groups.RemoveRange(groups);
        await this.DBContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    #region Inject

    private ProjectDbContext DBContext { get; }

    public GroupRepository(ProjectDbContext dbContext)
    {
        this.DBContext = dbContext;
    }

    #endregion
}