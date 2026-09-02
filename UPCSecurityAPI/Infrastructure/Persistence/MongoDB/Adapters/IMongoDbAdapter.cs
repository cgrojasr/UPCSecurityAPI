using MongoDB.Driver;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;

public interface IMongoDbAdapter
{
    IMongoDatabase Database { get; }

    IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName);
}
