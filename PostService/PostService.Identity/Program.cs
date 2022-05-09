using Microsoft.AspNetCore.Identity;
using PostService.Common.App.Extensions;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Mongo.Extensions;
using PostService.Identity.Models.Domain;
using PostService.Common.CORS.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

// Configuration
builder.ConfigureAppOptions();

builder.AddJwt();
builder.AddCors();

builder.AddMongo();
builder.AddMongoRepository<User>("users");
builder.AddMongoRepository<RefreshToken>("refresh-tokens");

builder.RegisterProviders();

// Application
var app = builder.Build();
app.UseCors();

app.MapDefaultControllerRoute();
app.Run();
