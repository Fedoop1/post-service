﻿using EasyNetQ;
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
            new MessageNamingConventionProvider(provider.GetService<IOptions<RabbitMqOptions>>()!,
                new DefaultTypeNameSerializer()));

        webApplicationBuilder.Services
            .AddSingleton<IMessageNamingConventionProvider>(provider =>
                provider.GetService<MessageNamingConventionProvider>());

        webApplicationBuilder.Services
            .AddSingleton<ITypeNameSerializer, TypeNameSerializer>();

        webApplicationBuilder.Services.AddSingleton(provider =>
        {
            var rabbitMqOptions = provider.GetService<IOptions<RabbitMqOptions>>()!.Value;
            var namingConventionProvider = provider.GetService<MessageNamingConventionProvider>();
            var typeNameSerializer = provider.GetService<ITypeNameSerializer>();

            var busPublisher = RabbitHutch.CreateBus(rabbitMqOptions.HostName, (ushort)rabbitMqOptions.Port,
                rabbitMqOptions.VirtualHost, rabbitMqOptions.UserName, rabbitMqOptions.Password,
                rabbitMqOptions.HeartbeatInterval, register =>
                {
                    register.Register<IConventions>(namingConventionProvider);
                    register.Register<ITypeNameSerializer>(typeNameSerializer);
                });

            return busPublisher;
        });

        webApplicationBuilder.Services.AddSingleton<IBusPublisher, BusPublisher>();
    }

    public static IBusSubscriber UseRabbitMq(this WebApplication webApp) => new BusSubscriber(webApp);

    private static void ConfigureRabbitMqOptions(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<RabbitMqOptions>().BindConfiguration(SectionName);
}