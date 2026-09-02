using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Options;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;

public class MongoDbAdapter : IMongoDbAdapter
{
    public MongoDbAdapter(IOptions<MongoDbOptions> options)
    {
        var settings = options.Value;

        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new InvalidOperationException("MongoDb:ConnectionString no está configurado.");
        }

        if (string.IsNullOrWhiteSpace(settings.DatabaseName))
        {
            throw new InvalidOperationException("MongoDb:DatabaseName no está configurado.");
        }

        var client = new MongoClient(settings.ConnectionString);
        Database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
    {
        return Database.GetCollection<TDocument>(collectionName);
    }
}
