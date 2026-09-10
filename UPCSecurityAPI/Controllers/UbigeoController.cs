using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPCSecurityAPI.Application.DTOs.Region;
using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.CrossCutting.Exceptions;
using UPCSecurityAPI.CrossCutting.Responses;

namespace UPCSecurityAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UbigeoController : ControllerBase
{
    private readonly IRegionService _regionService;
    private readonly IProvinciaService _provinciaService;
    private readonly IDistritoService _distritoService;

    public UbigeoController(IRegionService regionService, IProvinciaService provinciaService, IDistritoService distritoService)
    {
        _regionService = regionService;
        _provinciaService = provinciaService;
        _distritoService = distritoService;
    }

    [Authorize]
    [HttpGet("regiones")]
    [ProducesResponseType(typeof(List<RegionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllRegiones(CancellationToken cancellationToken)
    {
        var regiones = await _regionService.GetAllAsync(cancellationToken);
        return Ok(regiones);
    }

    [Authorize]
    [HttpGet("provincias")]
    [ProducesResponseType(typeof(List<ProvinciaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllProvinciasByCodigoRegionAsync([FromQuery] string codigoRegion, CancellationToken cancellationToken)
    {
        try
        {
            var provincias = await _provinciaService.GetAllProvinciasByCodigoRegionAsync(codigoRegion, cancellationToken);
            return Ok(provincias);
        }
        catch (AppValidationException ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Errors = ex.Errors
            });
        }
    }

    [Authorize]
    [HttpGet("distritos")]
    [ProducesResponseType(typeof(List<DistritoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllDistritoByCodigoProvinciaAsync([FromQuery] string codigoProvincia, CancellationToken cancellationToken)
    {
        try
        {
            var distritos = await _distritoService.GetAllDistritoByCodigoProvinciaAsync(codigoProvincia, cancellationToken);
            return Ok(distritos);
        }
        catch (AppValidationException ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Errors = ex.Errors
            });
        }
    }
}
