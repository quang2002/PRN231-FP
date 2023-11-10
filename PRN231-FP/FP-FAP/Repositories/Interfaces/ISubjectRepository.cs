namespace FP_FAP.Repositories.Interfaces;

using FP_FAP.Models;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllSubjectsAsync(CancellationToken cancellationToken);

    Task<Subject?> GetSubjectByIdAsync(int               id,
                                       CancellationToken cancellationToken);

    Task<bool> CreateSubjectAsync(Subject           subject,
                                  CancellationToken cancellationToken);
}