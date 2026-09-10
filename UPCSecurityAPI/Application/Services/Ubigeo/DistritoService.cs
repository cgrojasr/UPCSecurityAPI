using UPCSecurityAPI.Application.DTOs.Region;
using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.CrossCutting.Exceptions;
using UPCSecurityAPI.Domain.Interfaces.Repositories;

namespace UPCSecurityAPI.Application.Services.Region;

public class DistritoService : IDistritoService
{
    private readonly IDistritoRepository _distritoRepository;

    public DistritoService(IDistritoRepository distritoRepository)
    {
        _distritoRepository = distritoRepository;
    }

    public async Task<List<DistritoDto>> GetAllDistritoByCodigoProvinciaAsync(string codigoProvincia, CancellationToken cancellationToken = default)
    {
        var trimmedCodigoProvincia = codigoProvincia?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(trimmedCodigoProvincia))
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["codigoProvincia"] = ["El código de provincia es requerido."]
            });
        }

        var distritos = await _distritoRepository.GetAllByCodigoProvinciaAsync(trimmedCodigoProvincia, cancellationToken);

        return distritos
            .Select(distrito => new DistritoDto
            {
                Codigo = distrito.Codigo,
                Nombre = distrito.Nombre
            })
            .ToList();
    }
}
