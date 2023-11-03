namespace FP_FAP.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Subject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonRequired]
    [BsonElement("code")]
    public string? Code { get; set; }

    [BsonRequired]
    [BsonElement("name")]
    public string? Name { get; set; }
}