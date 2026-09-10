using UPCSecurityAPI.Application.DTOs.Region;

namespace UPCSecurityAPI.Application.Interfaces.Services;

public interface IProvinciaService
{
    Task<List<ProvinciaDto>> GetAllProvinciasByCodigoRegionAsync(string codigoRegion, CancellationToken cancellationToken = default);
}
