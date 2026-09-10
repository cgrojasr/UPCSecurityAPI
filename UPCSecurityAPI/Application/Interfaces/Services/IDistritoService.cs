using UPCSecurityAPI.Application.DTOs.Region;

namespace UPCSecurityAPI.Application.Interfaces.Services;

public interface IDistritoService
{
    Task<List<DistritoDto>> GetAllDistritoByCodigoProvinciaAsync(string codigoProvincia, CancellationToken cancellationToken = default);
}
