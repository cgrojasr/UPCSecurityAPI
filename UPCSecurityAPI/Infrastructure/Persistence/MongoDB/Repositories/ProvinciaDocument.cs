using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;

[BsonIgnoreExtraElements]
public class ProvinciaDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("cod_region")]
    public string CodigoRegion { get; set; } = string.Empty;
}
