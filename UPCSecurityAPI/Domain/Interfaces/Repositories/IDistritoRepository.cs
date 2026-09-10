using UPCSecurityAPI.Domain.Entities;

namespace UPCSecurityAPI.Domain.Interfaces.Repositories;

public interface IDistritoRepository
{
    Task<List<Distrito>> GetAllByCodigoProvinciaAsync(string codigoProvincia, CancellationToken cancellationToken = default);
}
