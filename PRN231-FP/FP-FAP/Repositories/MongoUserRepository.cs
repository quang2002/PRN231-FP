namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using MongoDB.Driver;

public class MongoUserRepository : IUserRepository
{
    public MongoUserRepository(MongoDBContext mongoDBContext)
    {
        this.MongoDBContext = mongoDBContext;
    }

    private MongoDBContext MongoDBContext { get; }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var cursor = await this.MongoDBContext.Users.FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id.ToString(), id);
        var cursor = await this.MongoDBContext.Users.FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User> GetOrCreateByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await this.GetByEmailAsync(email, cancellationToken);
        if (user is not null)
        {
            return user;
        }

        user = new()
        {
            Email = email,
            Role  = Roles.Student,
        };

        await this.MongoDBContext.Users.InsertOneAsync(user, cancellationToken: cancellationToken);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Empty;
        var result = await this.MongoDBContext.Users.FindAsync(filter, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken);
    }
}