using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PostService.Common.Mongo.Types;
using PostService.Common.Types;

namespace PostService.Common.Mongo.Extensions;
public static class MongoExtensions
{
    public static void AddMongo(this WebApplicationBuilder webBuilder)
    {
        using var serviceProvider = webBuilder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        webBuilder.Services.Configure<MongoOptions>(configuration.GetSection("Mongo"));

        var mongoOptions = serviceProvider.GetService<MongoOptions>();
        var mongoClinet = new MongoClient(mongoOptions.connectionString);
        var database = mongoClinet.GetDatabase(mongoOptions.databaseName);

        webBuilder.Services.AddSingleton(database);
    }

    public static void AddMongoRepository<TEntity>(this WebApplicationBuilder webBuilder, string collectionName) where TEntity : IIdentifiable
    {
        webBuilder.Services.AddScoped<IMongoRepository<TEntity>>((config) =>
        {
            var mongoDb = config.GetService<IMongoDatabase>();
            return new MongoRepository<TEntity>(mongoDb, collectionName);
        });
    }
}
