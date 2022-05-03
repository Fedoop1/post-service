using PostService.API.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.ConfigureAppOptions();

builder.Services.AddCors((options) =>
{
    options.AddPolicy("Development", (builder) =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
        builder.AllowCredentials();
    });

    options.DefaultPolicyName = "Development";
});

builder.Services.AddAuthorization(options => options.AddPolicy("Admin", builder => builder.RequireRole("admin")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseCors(app.Configuration.GetValue<string>("CORS"));

app.MapControllers();

app.Run();
