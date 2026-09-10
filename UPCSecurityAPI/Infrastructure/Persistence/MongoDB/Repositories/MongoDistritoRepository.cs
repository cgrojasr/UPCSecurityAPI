using MongoDB.Driver;
using UPCSecurityAPI.Domain.Entities;
using UPCSecurityAPI.Domain.Interfaces.Repositories;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Context;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;

public class MongoDistritoRepository : IDistritoRepository
{
    private readonly IMongoCollection<DistritoDocument> _distritos;

    public MongoDistritoRepository(IMongoDbAdapter mongoDbAdapter)
    {
        _distritos = mongoDbAdapter.GetCollection<DistritoDocument>(MongoCollections.Distritos);
    }

    public async Task<List<Distrito>> GetAllByCodigoProvinciaAsync(string codigoProvincia, CancellationToken cancellationToken = default)
    {
        var filter = Builders<DistritoDocument>.Filter.Eq(distrito => distrito.CodProvincia, codigoProvincia);

        var documents = await _distritos
            .Find(filter)
            .ToListAsync(cancellationToken);

        return documents.Select(MapToDomain).ToList();
    }

    private static Distrito MapToDomain(DistritoDocument document)
    {
        return new Distrito
        {
            Codigo = document.Codigo,
            Nombre = document.Nombre
        };
    }
}
