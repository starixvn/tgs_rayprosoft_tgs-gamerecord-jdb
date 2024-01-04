using System.Reflection;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameRecord.JDB;
using Tgs.GameRecord.JDB.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecord(
       this IServiceCollection services,
       Action<RedisOptions, IServiceProvider> redisDBconfigure,
       Action<GameRecordInfoOptions, IServiceProvider> gameRecordInfoOptions,
       Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
       Action<GameInterfaceJDBClientOptions, IServiceProvider> gameInterfaceJDBClientOptions)
        => services
        .AddOptions<RedisOptions>()
        .Configure(redisDBconfigure)
        .Services
        .AddOptions<GameRecordInfoOptions>()
        .Configure(gameRecordInfoOptions)
        .Services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddSingleton<RedisConnectionManager>()
        .AddGameRecordCore(
            gameCommandClientOptions,
            gameInterfaceJDBClientOptions,
            redisDatabaseResolver: (sp) => sp.GetRequiredService<RedisConnectionManager>().Database);
}
