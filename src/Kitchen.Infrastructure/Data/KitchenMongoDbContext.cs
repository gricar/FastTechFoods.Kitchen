using Kitchen.Application.Infrastructure.Data;
using Kitchen.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Kitchen.Infrastructure.Data;

public class KitchenMongoDbContext : IKitchenMongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<KitchenMongoDbContext> _logger;

    public KitchenMongoDbContext(IMongoClient client, string databaseName, ILogger<KitchenMongoDbContext> logger)
    {
        _logger = logger;
        _database = client.GetDatabase(databaseName);
        _logger.LogInformation("MongoDB context initialized for database: {DatabaseName}", databaseName);
    }

    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
}
