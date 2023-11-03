namespace FP_FAP.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Feedback
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonRequired]
    [BsonElement("student_id")]
    public ObjectId StudentId { get; set; }

    [BsonRequired]
    [BsonElement("group_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId GroupId { get; set; }

    [BsonElement("is_done")]
    public bool IsDone { get; set; }

    [BsonElement("punctuality")]
    public int Punctuality { get; set; }

    [BsonElement("skill")]
    public int Skill { get; set; }

    [BsonElement("adequately")]
    public int Adequately { get; set; }

    [BsonElement("support")]
    public int Support { get; set; }

    [BsonElement("response")]
    public int Response { get; set; }

    [BsonElement("comment")]
    public string? Comment { get; set; }
}