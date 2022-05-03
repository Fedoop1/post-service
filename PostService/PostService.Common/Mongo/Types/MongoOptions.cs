using System;

namespace PostService.Common.Mongo.Types;

public record MongoOptions(string connectionString, string databaseName);
