using Kitchen.Application.Infrastructure.Data;
using Kitchen.Application.Infrastructure.Services;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;
using Kitchen.Infrastructure.Data;
using Kitchen.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Kitchen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var connectionString = configuration.GetConnectionString("MongoDb") ?? throw new ArgumentNullException("MongoDbConnection", "MongoDB connection string is missing.");
            if (string.IsNullOrEmpty(connectionString))
            {
                var logger = sp.GetRequiredService<ILogger<IMongoClient>>();
                logger.LogError("MongoDB connection string 'MongoDbConnection' is not configured.");
                throw new ArgumentNullException("MongoDbConnection", "MongoDB connection string is missing.");
            }
            return new MongoClient(connectionString);
        });

        services.AddSingleton<IKitchenMongoDbContext, KitchenMongoDbContext>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration["MongoDb:DatabaseName"] ?? "FastTechFoodsKitchenDb";
            var logger = sp.GetRequiredService<ILogger<KitchenMongoDbContext>>();
            return new KitchenMongoDbContext(mongoClient, databaseName, logger);
        });

        // Configure BSON Serialization for your domain entities
        ConfigureBsonSerialization();

        services.AddSingleton<IEventBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var uri = configuration.GetConnectionString("RabbitMq") ?? throw new ArgumentNullException("RabbitMqConnection", "RabbitMq connection string is missing.");
            var connectionName = configuration["MessageBroker:ConnectionName"] ?? "Kitchen.API";
            return new RabbitMQEventBus(uri, connectionName, logger, sp);
        });

        return services;
    }

    private static void ConfigureBsonSerialization()
    {
        // Register Guid Id Generator for MongoDB
        BsonSerializer.RegisterIdGenerator(typeof(Guid), new GuidGenerator());

        // Map your Order entity
        if (!BsonClassMap.IsClassMapRegistered(typeof(Order)))
        {
            BsonClassMap.RegisterClassMap<Order>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true); // Ignore any fields in DB not present in the model
                cm.MapIdProperty(o => o.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));  // Map the Id property as the BSON _id
                cm.MapProperty(o => o.CustomerId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard)).SetElementName("CustomerId");
                cm.MapProperty(o => o.OrderItems).SetElementName("OrderItems");
                cm.MapProperty(o => o.TotalPrice).SetElementName("TotalPrice");
                cm.MapProperty(o => o.Status).SetElementName("Status")
                    .SetSerializer(new MongoDB.Bson.Serialization.Serializers.EnumSerializer<OrderStatus>(MongoDB.Bson.BsonType.String)); // Store enum as string
                cm.MapProperty(o => o.LastModified).SetElementName("LastModified");
                cm.MapProperty(o => o.LastModifiedBy).SetElementName("LastModifiedBy");
            });
        }

        // Map your OrderItem entity
        if (!BsonClassMap.IsClassMapRegistered(typeof(OrderItem)))
        {
            BsonClassMap.RegisterClassMap<OrderItem>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapProperty(oi => oi.OrderId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard)).SetElementName("OrderId");
                cm.MapProperty(oi => oi.ProductId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard)).SetElementName("ProductId");
                cm.MapProperty(oi => oi.Quantity).SetElementName("Quantity");
                cm.MapProperty(oi => oi.Price).SetElementName("Price");
            });
        }
    }
}
