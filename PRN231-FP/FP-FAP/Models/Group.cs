namespace FP_FAP.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Group
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonRequired]
    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonRequired]
    [BsonElement("subject_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId SubjectId { get; set; }

    [BsonRequired]
    [BsonElement("teacher_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId TeacherId { get; set; }

    [BsonElement("students")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId[] Students { get; set; } = Array.Empty<ObjectId>();
}