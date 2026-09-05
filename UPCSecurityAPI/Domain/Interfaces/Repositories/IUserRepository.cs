using UPCSecurityAPI.Domain.Entities;

namespace UPCSecurityAPI.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetByNroDocumentoAsync(string nroDocumento, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailOrNroDocumentoAsync(string identifier, CancellationToken cancellationToken = default);

    Task CreateAsync(User user, CancellationToken cancellationToken = default);
}
