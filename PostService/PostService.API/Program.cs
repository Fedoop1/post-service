using PostService.Common.App.Extensions;
using PostService.Common.CORS.Extensions;
using PostService.Common.Jwt.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ConfigureAppOptions();

builder.AddJwt();
builder.AddCors();

builder.Services.AddAuthorization(options => options.AddPolicy("Admin", builder => builder.RequireRole("admin")));

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
