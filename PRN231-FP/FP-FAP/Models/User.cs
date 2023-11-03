namespace FP_FAP.Models;

using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Roles
{
    public const string Student = "student";
    public const string Teacher = "teacher";
    public const string Admin   = "admin";
}

public class User
{
    [BsonId]
    [JsonPropertyName("id")]
    public ObjectId Id { get; set; }

    [BsonRequired]
    [BsonElement("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [BsonElement("role")]
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
}