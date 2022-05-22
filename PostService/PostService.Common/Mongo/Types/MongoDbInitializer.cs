using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace PostService.Common.Mongo.Types;
public class MongoDbInitializer : IMongoDbInitializer
{
    public Task InitializeAsync()
    {
        RegisterConventions();

        return Task.CompletedTask;
    }

    public static void RegisterConventions()
    {
        ConventionRegistry.Register("Conventions", new MongoDbConventions(), _ => true);
    }
}

public class MongoDbConventions : IConventionPack
{
    public IEnumerable<IConvention> Conventions => new IConvention[]
    {
        new IgnoreExtraElementsConvention(true),
        new EnumRepresentationConvention(BsonType.String),
        new CamelCaseElementNameConvention(),
    };
}
