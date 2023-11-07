namespace FP_FAP.Repositories.Interfaces;

using FP_FAP.Models;
using MongoDB.Bson;

public interface IGroupRepository
{
    Task<IEnumerable<Group>> GetGroupsAsync(string?        semester, CancellationToken cancellationToken                             = default);
    Task<bool>               CreateGroupAsync(Group        group,    CancellationToken cancellationToken                             = default);
    Task<bool>               EnrollStudentAsync(ObjectId   groupId,  ObjectId[]        students, CancellationToken cancellationToken = default);
    Task<bool>               UnenrollStudentAsync(ObjectId groupId,  ObjectId[]        students, CancellationToken cancellationToken = default);
    Task<bool>               DeleteGroupAsync(ObjectId     groupId,  CancellationToken cancellationToken = default);
}