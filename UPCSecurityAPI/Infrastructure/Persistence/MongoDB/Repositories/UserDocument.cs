using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;

public class UserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("nroDocumento")]
    public string NroDocumento { get; set; } = string.Empty;

    [BsonElement("normalizedEmail")]
    public string NormalizedEmail { get; set; } = string.Empty;

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("role")]
    public string Role { get; set; } = "User";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
