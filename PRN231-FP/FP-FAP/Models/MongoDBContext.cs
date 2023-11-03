namespace FP_FAP.Models;

using MongoDB.Driver;

public class MongoDBContext
{
    public MongoDBContext(IMongoDatabase database)
    {
        this.Users     = database.GetCollection<User>("users");
        this.Groups    = database.GetCollection<Group>("groups");
        this.Subjects  = database.GetCollection<Subject>("subjects");
        this.Feedbacks = database.GetCollection<Feedback>("feedbacks");
    }

    public IMongoCollection<User>     Users     { get; }
    public IMongoCollection<Group>    Groups    { get; }
    public IMongoCollection<Subject>  Subjects  { get; }
    public IMongoCollection<Feedback> Feedbacks { get; }
}