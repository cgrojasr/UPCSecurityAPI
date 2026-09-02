using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.Application.Services.Auth;

namespace UPCSecurityAPI.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
