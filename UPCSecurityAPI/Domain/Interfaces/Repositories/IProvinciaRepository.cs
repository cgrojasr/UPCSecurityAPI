using UPCSecurityAPI.Domain.Entities;

namespace UPCSecurityAPI.Domain.Interfaces.Repositories;

public interface IProvinciaRepository
{
    Task<List<Provincia>> GetAllByCodigoRegionAsync(string codigoRegion, CancellationToken cancellationToken = default);
}
