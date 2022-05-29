using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PostService.Common.Mongo.Types;
using PostService.Common.Types;

namespace PostService.Common.Mongo.Extensions;
public static class MongoExtensions
{
    public const string SectionName = "Mongo";
    public static void AddMongo(this WebApplicationBuilder webBuilder)
    {
        webBuilder.ConfigureMongoOptions();

        using var serviceProvider = webBuilder.Services.BuildServiceProvider();

        var mongoOptions = serviceProvider.GetService<IOptions<MongoOptions>>()?.Value;

        var mongoClinet = new MongoClient(mongoOptions.ConnectionString);
        var database = mongoClinet.GetDatabase(mongoOptions.DatabaseName);

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

    private static void ConfigureMongoOptions(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<MongoOptions>().BindConfiguration(SectionName);
}
