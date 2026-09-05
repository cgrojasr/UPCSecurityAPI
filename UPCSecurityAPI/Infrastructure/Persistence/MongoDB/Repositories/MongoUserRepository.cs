using MongoDB.Bson;
using MongoDB.Driver;
using UPCSecurityAPI.Domain.Entities;
using UPCSecurityAPI.Domain.Interfaces.Repositories;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Adapters;
using UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Context;

namespace UPCSecurityAPI.Infrastructure.Persistence.MongoDB.Repositories;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<UserDocument> _users;
    private static readonly object IndexLock = new();
    private static bool _indexesCreated;

    public MongoUserRepository(IMongoDbAdapter mongoDbAdapter)
    {
        _users = mongoDbAdapter.GetCollection<UserDocument>(MongoCollections.Users);
        EnsureIndexesCreated();
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = (email ?? string.Empty).Trim().ToLowerInvariant();

        var document = await _users
            .Find(user => user.NormalizedEmail == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);

        return document is null ? null : MapToDomain(document);
    }

    public async Task<User?> GetByNroDocumentoAsync(string nroDocumento, CancellationToken cancellationToken = default)
    {
        var trimmedNroDocumento = (nroDocumento ?? string.Empty).Trim();

        var document = await _users
            .Find(user => user.NroDocumento == trimmedNroDocumento)
            .FirstOrDefaultAsync(cancellationToken);

        return document is null ? null : MapToDomain(document);
    }

    public async Task<User?> GetByEmailOrNroDocumentoAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var normalizedIdentifier = (identifier ?? string.Empty).Trim().ToLowerInvariant();
        var trimmedIdentifier = (identifier ?? string.Empty).Trim();

        var document = await _users
            .Find(user => user.NormalizedEmail == normalizedIdentifier || user.NroDocumento == trimmedIdentifier)
            .FirstOrDefaultAsync(cancellationToken);

        return document is null ? null : MapToDomain(document);
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var document = MapToDocument(user);
        await _users.InsertOneAsync(document, cancellationToken: cancellationToken);
        user.Id = document.Id;
    }

    private void EnsureIndexesCreated()
    {
        if (_indexesCreated)
        {
            return;
        }

        lock (IndexLock)
        {
            if (_indexesCreated)
            {
                return;
            }

            var emailIndexKeys = Builders<UserDocument>.IndexKeys.Ascending(user => user.NormalizedEmail);
            var emailIndexOptions = new CreateIndexOptions { Name = "ux_users_normalized_email", Unique = true };
            var emailIndexModel = new CreateIndexModel<UserDocument>(emailIndexKeys, emailIndexOptions);

            var nroDocumentoIndexKeys = Builders<UserDocument>.IndexKeys.Ascending(user => user.NroDocumento);
            var nroDocumentoIndexOptions = new CreateIndexOptions { Name = "ux_users_nro_documento", Unique = true };
            var nroDocumentoIndexModel = new CreateIndexModel<UserDocument>(nroDocumentoIndexKeys, nroDocumentoIndexOptions);

            _users.Indexes.CreateMany([emailIndexModel, nroDocumentoIndexModel]);
            _indexesCreated = true;
        }
    }

    private static UserDocument MapToDocument(User user)
    {
        var normalizedEmail = (user.Email ?? string.Empty).Trim().ToLowerInvariant();

        return new UserDocument
        {
            Id = string.IsNullOrWhiteSpace(user.Id) ? ObjectId.GenerateNewId().ToString() : user.Id,
            Email = user.Email ?? string.Empty,
            NroDocumento = user.NroDocumento ?? string.Empty,
            NormalizedEmail = normalizedEmail,
            PasswordHash = user.PasswordHash ?? string.Empty,
            Role = user.Role ?? "User",
            CreatedAt = user.CreatedAt
        };
    }

    private static User MapToDomain(UserDocument document)
    {
        return new User
        {
            Id = document.Id ?? string.Empty,
            Email = document.Email ?? string.Empty,
            NroDocumento = document.NroDocumento ?? string.Empty,
            PasswordHash = document.PasswordHash ?? string.Empty,
            Role = document.Role ?? "User",
            CreatedAt = document.CreatedAt
        };
    }
}
