using UPCSecurityAPI.Application.DTOs.Region;

namespace UPCSecurityAPI.Application.Interfaces.Services;

public interface IRegionService
{
    Task<List<RegionDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
