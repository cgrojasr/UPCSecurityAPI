using UPCSecurityAPI.Application.DTOs.Region;
using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.Domain.Interfaces.Repositories;

namespace UPCSecurityAPI.Application.Services.Region;

public class RegionService : IRegionService
{
    private readonly IRegionRepository _regionRepository;

    public RegionService(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public async Task<List<RegionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var regions = await _regionRepository.GetAllAsync(cancellationToken);

        return regions
            .Select(region => new RegionDto
            {
                Codigo = region.Codigo,
                Nombre = region.Nombre
            })
            .ToList();
    }
}
