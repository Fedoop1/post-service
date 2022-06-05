using PostService.Common.App.Extensions;
using PostService.Common.CORS.Extensions;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Mongo.Extensions;
using PostService.Common.RabbitMq.Extensions;
using PostService.Common.Redis.Extensions;
using PostService.Operations.Extensions.RabbitMq;
using PostService.Operations.Messages.Events.Operations;
using PostService.Operations.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddJwt();
builder.AddCors();
builder.AddRedis();

builder.AddMongo();
builder.AddMongoRepository<Operation>("operations");

builder.AddRabbitMq();
builder.AddGenericEventHandler();

builder.RegisterProviders();

var app = builder.Build();

app.UseCors();
app.UseRabbitMq()
    .SubscribeToAllEvents(new [] {typeof(OperationPending), typeof(OperationCompleted), typeof(OperationRejected) });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();