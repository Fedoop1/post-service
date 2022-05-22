﻿using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
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
        webApplicationBuilder.Services.AddOptions<RabbitMqOptions>(SectionName);

        using var services = webApplicationBuilder.Services.BuildServiceProvider();

        webApplicationBuilder.Services.AddSingleton<IBus>(provider =>
        {
            var rabbitMqOptions = provider.GetService<IOptions<RabbitMqOptions>>().Value;

            var busPublisher = RabbitHutch.CreateBus(rabbitMqOptions.HostName, (ushort)rabbitMqOptions.Port,
                rabbitMqOptions.VirtualHost, rabbitMqOptions.UserName, rabbitMqOptions.Password,
                rabbitMqOptions.HeartbeatInterval, register => { });

            ConfigureBus(busPublisher);

            return busPublisher;
        });
    }

    // TODO: Add additional abstraction level over IBus
    private static void ConfigureBus(IBus busPublisher)
    {
        var autoSubscriber = new AutoSubscriber(busPublisher, "")
        {
            // TODO: Add message dispatcher
            AutoSubscriberMessageDispatcher = null,
            ConfigureSubscriptionConfiguration = config =>
            {
                // TODO: Add config here
            }
        };

        autoSubscriber.Subscribe(new[] { Assembly.GetCallingAssembly() });
    }
}