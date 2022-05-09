namespace PostService.Common.Mongo.Types;

public record MongoOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
