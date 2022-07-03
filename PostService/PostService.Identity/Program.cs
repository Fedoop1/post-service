using Microsoft.AspNetCore.Identity;
using PostService.Common.App.Extensions;
using PostService.Common.Consul.Extensions;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Mongo.Extensions;
using PostService.Identity.Models.Domain;
using PostService.Common.CORS.Extensions;
using PostService.Common.Extensions;
using PostService.Common.Logging.Extensions;
using PostService.Common.Mongo.Types;
using PostService.Common.RabbitMq.Extensions;
using PostService.Common.Redis.Extensions;
using PostService.Common.Seq.Extensions;
using PostService.Common.Tracing.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IMongoDbInitializer, MongoDbInitializer>();

// Configuration
builder.ConfigureAppOptions();

builder.AddJwt();
builder.AddAccessTokenValidation();
builder.AddRedis();
builder.AddCors();
builder.AddConsul();
builder.AddRabbitMq();
builder.AddConsoleLoggingFormatter();
builder.AddControllerLogging();
builder.AddSeq();
builder.AddTracing();

builder.AddMongo();
builder.AddMongoRepository<User>("users");
builder.AddMongoRepository<RefreshToken>("refresh-tokens");

builder.AddStartupInitializer(typeof(IMongoDbInitializer));

builder.RegisterProviders();

// Application
var app = builder.Build();

app.UseTracing();
app.UseCors();
app.UseConsul();

app.UseRabbitMq();

app.UseAuthentication();
app.UseAuthorization();
app.UseAccessTokenValidation();

app.InitializeAsync();

app.MapDefaultControllerRoute();
app.Run();
