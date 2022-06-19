using PostService.Common.App.Extensions;
using PostService.Common.Consul.Extensions;
using PostService.Common.CORS.Extensions;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Redis.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ConfigureAppOptions();

builder.AddRedis();
builder.AddJwt();
builder.AddAccessTokenValidation();
builder.AddCors();
builder.AddConsul();

builder.Services.AddAuthorization(options => options.AddPolicy("Admin", builder => builder.RequireRole("admin")));

var app = builder.Build();

app.UseCors();
app.UseConsul();

app.UseAuthentication();
app.UseAuthorization();
app.UseAccessTokenValidation();

app.MapDefaultControllerRoute();
app.Run();
