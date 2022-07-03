using PostService.Common.App.Extensions;
using PostService.Common.Consul.Extensions;
using PostService.Common.CORS.Extensions;
using PostService.Common.Jwt.Extensions;
using PostService.Common.Logging.Extensions;
using PostService.Common.Mongo.Extensions;
using PostService.Common.RabbitMq.Extensions;
using PostService.Common.Redis.Extensions;
using PostService.Common.Seq.Extensions;
using PostService.Common.Tracing.Extensions;
using PostService.Operations.Extensions.RabbitMq;
using PostService.Operations.Messages.Events.Operations;
using PostService.Operations.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddJwt();
builder.AddCors();
builder.AddConsul();
builder.AddRedis();
builder.AddControllerLogging();
builder.AddConsoleLoggingFormatter();
builder.AddSeq();
builder.AddTracing();

builder.AddMongo();
builder.AddMongoRepository<Operation>("operations");

builder.AddRabbitMq();
builder.AddGenericEventHandler();

builder.RegisterProviders();

var app = builder.Build();

app.UseTracing();
app.UseCors();
app.UseRabbitMq()
    .SubscribeToAllEvents(new [] {typeof(OperationPending), typeof(OperationCompleted), typeof(OperationRejected) });

app.UseAuthentication();
app.UseAuthorization();

app.UseConsul();

app.MapDefaultControllerRoute();

app.Run();