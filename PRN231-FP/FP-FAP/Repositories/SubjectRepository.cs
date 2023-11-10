namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class SubjectRepository : ISubjectRepository
{
    public async Task<IEnumerable<Subject>> GetAllSubjectsAsync(CancellationToken cancellationToken)
    {
        return await this.DBContext.Subjects.ToListAsync(cancellationToken);
    }

    public async Task<Subject?> GetSubjectByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await this.DBContext.Subjects.FindAsync(new[] { id }, cancellationToken);
    }

    public async Task<bool> CreateSubjectAsync(Subject subject, CancellationToken cancellationToken)
    {
        try
        {
            await this.DBContext.Subjects.AddAsync(subject, cancellationToken);
            await this.DBContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #region Inject

    private ProjectDbContext DBContext { get; }

    public SubjectRepository(ProjectDbContext dbContext)
    {
        this.DBContext = dbContext;
    }

    #endregion
}