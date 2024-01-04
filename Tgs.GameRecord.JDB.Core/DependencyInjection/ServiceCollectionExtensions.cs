using System.Reflection;
using StackExchange.Redis;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameRecord.JDB.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecordCore(
        this IServiceCollection services,
        Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
        Action<GameInterfaceJDBClientOptions, IServiceProvider> gameInterfaceJDBClientOptions,
        Func<IServiceProvider, IDatabase> redisDatabaseResolver)
        => services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddGameCommandClient(gameCommandClientOptions)
        .AddGameInterfaceJDBServiceClient(gameInterfaceJDBClientOptions)
        .AddGameRecordHandleClient()
        .AddTransient<IHangfireUtensil, HangfireUtensil>()
        .AddTransient<IHangfireLock>(sp => ActivatorUtilities.CreateInstance<HangfireLock>(sp, redisDatabaseResolver(sp)))
        .AddSingleton<IUtensil, UtensilRepository>()
        .AddSingleton<ISystemClock, SystemClock>();
}
