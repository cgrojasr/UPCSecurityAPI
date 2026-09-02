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

            var indexKeys = Builders<UserDocument>.IndexKeys.Ascending(user => user.Email);
            var indexOptions = new CreateIndexOptions { Name = "ux_users_email", Unique = true };
            var indexModel = new CreateIndexModel<UserDocument>(indexKeys, indexOptions);
            _users.Indexes.CreateOne(indexModel);
            _indexesCreated = true;
        }
    }

    private static UserDocument MapToDocument(User user)
    {
        var normalizedEmail = (user.Email ?? string.Empty).Trim().ToLowerInvariant();

        return new UserDocument
        {
            Id = string.IsNullOrWhiteSpace(user.Id) ? ObjectId.GenerateNewId().ToString() : user.Id,
            Email = user.Email,
            NormalizedEmail = normalizedEmail,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    private static User MapToDomain(UserDocument document)
    {
        return new User
        {
            Id = document.Id,
            Email = document.Email,
            PasswordHash = document.PasswordHash,
            Role = document.Role,
            CreatedAt = document.CreatedAt
        };
    }
}
