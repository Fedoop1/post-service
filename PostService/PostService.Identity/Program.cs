using PostService.Identity.Infrastructure.Extensions;
using PostService.Identity.Models.Domain;
using PostService.Identity.Services;
using PostService.Identity.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddScoped<IIdentityService, IdentityService>();

// Configuration
builder.ConfigureAppOptions();
builder.AddMongo();
builder.AddMongoRepository<User>("users");
builder.AddMongoRepository<User>("refresh-tokens");


builder.Services.AddCors((options) =>
{
    options.AddPolicy("Development", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
        builder.AllowCredentials();
    });

    options.DefaultPolicyName = "Development";
});

// Application
var app = builder.Build();
app.UseCors(app.Configuration.GetValue<string>("CORS"));
app.MapControllers();
app.Run();
