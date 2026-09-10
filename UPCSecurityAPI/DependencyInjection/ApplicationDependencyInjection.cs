using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.Application.Services.Auth;
using UPCSecurityAPI.Application.Services.Region;

namespace UPCSecurityAPI.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRegionService, RegionService>();
        services.AddScoped<IProvinciaService, ProvinciaService>();
        services.AddScoped<IDistritoService, DistritoService>();
        return services;
    }
}
