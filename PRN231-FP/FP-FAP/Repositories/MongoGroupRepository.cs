namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

public class MongoGroupRepository : IGroupRepository
{
    public async Task<IEnumerable<Group>> GetGroupsAsync(string? semester, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Group>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(semester))
        {
            semester =  semester.ToLower();
            filter   &= Builders<Group>.Filter.Eq(x => x.Semester.ToLower(), semester);
        }

        var result = await this.DBContext.Groups.FindAsync(filter, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken: cancellationToken);
    }

    public async Task<bool> CreateGroupAsync(Group group, CancellationToken cancellationToken = default)
    {
        var conflictFilter = Builders<Group>.Filter.Eq(x => x.Name, group.Name) &
                             Builders<Group>.Filter.Eq(x => x.Semester, group.Semester) &
                             Builders<Group>.Filter.Eq(x => x.SubjectId, group.SubjectId);

        var conflict = await this.DBContext.Groups.FindAsync(conflictFilter, cancellationToken: cancellationToken);

        if (await conflict.AnyAsync(cancellationToken: cancellationToken))
        {
            return false;
        }

        await this.DBContext.Groups.InsertOneAsync(group, cancellationToken: cancellationToken);
        return true;
    }

    public async Task<bool> EnrollStudentAsync(ObjectId groupId, ObjectId[] students, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Group>.Filter.Eq(x => x.Id, groupId);
        var update = Builders<Group>.Update.AddToSetEach(x => x.Students, students);

        var result = await this.DBContext.Groups.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> UnenrollStudentAsync(ObjectId groupId, ObjectId[] students, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Group>.Filter.Eq(x => x.Id, groupId);
        var update = Builders<Group>.Update.PullAll(x => x.Students, students);

        var result = await this.DBContext.Groups.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteGroupAsync(ObjectId groupId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Group>.Filter.Eq(x => x.Id, groupId);
        var result = await this.DBContext.Groups.DeleteOneAsync(filter, cancellationToken);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    #region Inject

    private MongoDBContext DBContext { get; }

    public MongoGroupRepository(MongoDBContext dbContext)
    {
        this.DBContext = dbContext;
    }

    #endregion
}