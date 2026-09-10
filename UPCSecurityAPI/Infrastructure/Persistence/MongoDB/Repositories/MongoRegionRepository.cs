using MongoDB.Driver;
using UPCSecurityAPI.Domain.Entities;
using UPCSecurityAPI.Domain.Interfaces.Repositories;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Context;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;

public class MongoRegionRepository : IRegionRepository
{
    private readonly IMongoCollection<RegionDocument> _regions;

    public MongoRegionRepository(IMongoDbAdapter mongoDbAdapter)
    {
        _regions = mongoDbAdapter.GetCollection<RegionDocument>(MongoCollections.Regions);
    }

    public async Task<List<Region>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var documents = await _regions
            .Find(FilterDefinition<RegionDocument>.Empty)
            .ToListAsync(cancellationToken);

        return documents.Select(MapToDomain).ToList();
    }

    private static Region MapToDomain(RegionDocument document)
    {
        return new Region
        {
            Codigo = document.Codigo,
            Nombre = document.Nombre
        };
    }
}
