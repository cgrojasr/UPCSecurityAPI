using MongoDB.Driver;
using UPCSecurityAPI.Domain.Entities;
using UPCSecurityAPI.Domain.Interfaces.Repositories;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Context;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;

public class MongoProvinciaRepository : IProvinciaRepository
{
    private readonly IMongoCollection<ProvinciaDocument> _provincias;

    public MongoProvinciaRepository(IMongoDbAdapter mongoDbAdapter)
    {
        _provincias = mongoDbAdapter.GetCollection<ProvinciaDocument>(MongoCollections.Provincias);
    }

    public async Task<List<Provincia>> GetAllByCodigoRegionAsync(string codigoRegion, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProvinciaDocument>.Filter.Eq(provincia => provincia.CodigoRegion, codigoRegion);

        var documents = await _provincias
            .Find(filter)
            .ToListAsync(cancellationToken);

        return documents.Select(MapToDomain).ToList();
    }

    private static Provincia MapToDomain(ProvinciaDocument document)
    {
        return new Provincia
        {
            Codigo = document.Codigo,
            Nombre = document.Nombre
        };
    }
}
