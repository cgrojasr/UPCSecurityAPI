using UPCSecurityAPI.Domain.Entities;

namespace UPCSecurityAPI.Domain.Interfaces.Repositories;

public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync(CancellationToken cancellationToken = default);
}
