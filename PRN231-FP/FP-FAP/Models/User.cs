namespace FP_FAP.Models;

using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [BsonElement("role")]
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
}

public class UserCollection
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var cursor = await this.Collection.FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var cursor = await this.Collection.FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User> GetOrCreateByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await this.GetByEmailAsync(email, cancellationToken);
        if (user is not null)
        {
            return user;
        }

        user = new User
        {
            Email = email,
            Role  = Roles.Student,
        };

        await this.Collection.InsertOneAsync(user, cancellationToken: cancellationToken);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cursor = await this.Collection.FindAsync(_ => true, cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    #region Inject

    public UserCollection(IMongoDatabase database)
    {
        this.Collection = database.GetCollection<User>("users");
    }

    private IMongoCollection<User> Collection { get; }

    #endregion
}