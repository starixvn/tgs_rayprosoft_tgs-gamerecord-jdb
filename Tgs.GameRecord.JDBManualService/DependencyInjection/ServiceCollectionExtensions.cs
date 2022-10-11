using Tgs.GameRecord.JDBManualService;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecordService(
        this IServiceCollection services,
        Action<RedisOptions, IServiceProvider> redisDBconfigure)
        => services
            .AddOptions<RedisOptions>()
            .Configure(redisDBconfigure)
            .Services
            .AddSingleton<RedisConnectionManager>();
}
