using UPCSecurityAPI.Domain.Interfaces.Adapters;
using UPCSecurityAPI.Domain.Interfaces.Repositories;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Options;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;
using UPCSecurityAPI.Infrastructure.Security.JWT.Builders;
using UPCSecurityAPI.Infrastructure.Security.JWT.Options;

namespace UPCSecurityAPI.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddSingleton<IMongoDbAdapter, MongoDbAdapter>();
        services.AddScoped<IUserRepository, MongoUserRepository>();
        services.AddScoped<IJwtTokenBuilder, JwtTokenBuilder>();

        return services;
    }
}
