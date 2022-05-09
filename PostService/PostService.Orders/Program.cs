using PostService.Common.App.Extensions;
using PostService.Common.CORS.Extensions;
using PostService.Common.Jwt.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddJwt();
builder.AddCors();

builder.ConfigureAppOptions();

var app = builder.Build();
app.UseCors();

app.MapDefaultControllerRoute();
app.Run();
