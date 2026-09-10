using UPCSecurityAPI.Application.DTOs.Region;
using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.CrossCutting.Exceptions;
using UPCSecurityAPI.Domain.Interfaces.Repositories;

namespace UPCSecurityAPI.Application.Services.Region;

public class ProvinciaService : IProvinciaService
{
    private readonly IProvinciaRepository _provinciaRepository;

    public ProvinciaService(IProvinciaRepository provinciaRepository)
    {
        _provinciaRepository = provinciaRepository;
    }

    public async Task<List<ProvinciaDto>> GetAllProvinciasByCodigoRegionAsync(string codigoRegion, CancellationToken cancellationToken = default)
    {
        var trimmedCodigoRegion = codigoRegion?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(trimmedCodigoRegion))
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["codigoRegion"] = ["El código de región es requerido."]
            });
        }

        var provincias = await _provinciaRepository.GetAllByCodigoRegionAsync(trimmedCodigoRegion, cancellationToken);

        return provincias
            .Select(provincia => new ProvinciaDto
            {
                Codigo = provincia.Codigo,
                Nombre = provincia.Nombre
            })
            .ToList();
    }
}
