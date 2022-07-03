using PostService.API.Services;
using PostService.Common.App.Extensions;
using PostService.Common.Consul.Extensions;
using PostService.Common.CORS.Extensions;
using PostService.Common.Jwt.Extensions;
using PostService.Common.LoadBalancing.Extensions;
using PostService.Common.Logging.Extensions;
using PostService.Common.Redis.Extensions;
using PostService.Common.Seq.Extensions;
using PostService.Common.Tracing.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.ConfigureAppOptions();

// Distributed cache
builder.AddRedis();

// Authentication
builder.AddJwt();
builder.AddAccessTokenValidation();
builder.AddCors();

// Logging
builder.AddControllerLogging();
builder.AddConsoleLoggingFormatter();
builder.AddSeq();
// Tracing
builder.AddTracing();

// Service discovering
builder.AddConsul();
builder.RegisterServiceForwarder<IOperationsService>("operations-service");

// Authorization
builder.Services.AddAuthorization(options => options.AddPolicy("Admin", builder => builder.RequireRole("admin")));

var app = builder.Build();

app.UseTracing();
app.UseCors();
app.UseConsul();

app.UseAuthentication();
app.UseAuthorization();
app.UseAccessTokenValidation();

app.MapDefaultControllerRoute();
app.Run();
