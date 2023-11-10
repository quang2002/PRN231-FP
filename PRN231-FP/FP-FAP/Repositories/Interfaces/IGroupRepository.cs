namespace FP_FAP.Repositories.Interfaces;

using FP_FAP.Models;

public interface IGroupRepository
{
    Task<IEnumerable<Group>> GetGroupsAsync(string?   semester, CancellationToken cancellationToken                             = default);
    Task<Group?>             GetGroupByIdAsync(int    id,       CancellationToken cancellationToken                             = default);
    Task<bool>               CreateGroupAsync(Group   group,    CancellationToken cancellationToken                             = default);
    Task<bool>               EnrollStudentAsync(int   groupId,  IEnumerable<int>  students, CancellationToken cancellationToken = default);
    Task<bool>               UnenrollStudentAsync(int groupId,  IEnumerable<int> students, CancellationToken cancellationToken = default);
    Task<bool>               DeleteGroupAsync(int[]   groupId,  CancellationToken cancellationToken = default);
}