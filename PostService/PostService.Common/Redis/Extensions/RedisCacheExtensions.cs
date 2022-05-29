using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostService.Common.Redis.Types;
using StackExchange.Redis;

namespace PostService.Common.Redis.Extensions;

public static class RedisCacheExtensions
{
    public const string SectionName = "Redis";

    public static void AddRedis(this WebApplicationBuilder webBuilder)
    {
        ConfigureRedisCache(webBuilder);

        using var providers = webBuilder.Services.BuildServiceProvider();
        var redisOptions = providers.GetService<IOptions<RedisCacheOptions>>().Value;

        webBuilder.Services.AddDistributedRedisCache(config =>
        {
            config.InstanceName = redisOptions.Instance;
            config.ConfigurationOptions = new ConfigurationOptions()
            {
                EndPoints = {redisOptions.ConnectionString},
                Password = redisOptions.Password,
                ClientName = redisOptions.ClientName,
            };
        });
    }

    private static void ConfigureRedisCache(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddOptions<RedisCacheOptions>().BindConfiguration(SectionName);
}
