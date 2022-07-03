using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostService.Common.Seq.Types;

namespace PostService.Common.Seq.Extensions;

public static class SeqExtensions
{
    public const string SectionName = "Seq";

    public static void AddSeq(this WebApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddOptions<SeqOptions>().BindConfiguration(SectionName);

        using var services = webBuilder.Services.BuildServiceProvider();
        var seqOptions = services.GetService<IOptions<SeqOptions>>()?.Value ??
                         throw new SeqOptionsNotDefinedException("Seq options not defined");

        webBuilder.Services.AddLogging(config =>
        {
            config.AddSeq(serverUrl: seqOptions.ServerUrl, apiKey: seqOptions.ApiKey);
        });
    }
}
