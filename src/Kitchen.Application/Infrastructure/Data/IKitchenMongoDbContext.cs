using Kitchen.Domain.Entities;
using MongoDB.Driver;

namespace Kitchen.Application.Infrastructure.Data;

public interface IKitchenMongoDbContext
{
    IMongoCollection<Order> Orders { get; }
}
