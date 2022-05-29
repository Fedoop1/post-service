using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostService.Common.RabbitMq.Types;

namespace PostService.Common.RabbitMq.Extensions;

public static class RabbitMqExtensions
{
    public const string SectionName = "RabbitMq";

    public static void AddRabbitMq(this WebApplicationBuilder webApplicationBuilder)
    {
        ConfigureRabbitMqOptions(webApplicationBuilder);

        webApplicationBuilder.Services.AddSingleton(provider =>
        {
            var rabbitMqOptions = provider.GetService<IOptions<RabbitMqOptions>>()!.Value;

            var busPublisher = RabbitHutch.CreateBus(rabbitMqOptions.HostName, (ushort)rabbitMqOptions.Port,
                rabbitMqOptions.VirtualHost, rabbitMqOptions.UserName, rabbitMqOptions.Password,
                rabbitMqOptions.HeartbeatInterval, register => { });

            return busPublisher;
        });

        webApplicationBuilder.Services.AddSingleton<IBusPublisher, BusPublisher>();
        webApplicationBuilder.Services.AddSingleton<IBusSubscriber, BusSubscriber>();
        webApplicationBuilder.Services
            .AddSingleton<IMessageNamingConventionProvider, MessageNamingConventionProvider>();
    }

    public static IBusSubscriber UseRabbitMq(this WebApplication webApp) => webApp.Services.GetService<IBusSubscriber>();

    private static void ConfigureRabbitMqOptions(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<RabbitMqOptions>().BindConfiguration(SectionName);

}