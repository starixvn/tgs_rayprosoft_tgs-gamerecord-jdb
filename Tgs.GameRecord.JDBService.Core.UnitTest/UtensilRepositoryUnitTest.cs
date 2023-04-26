using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;

namespace Tgs.GameRecord.JDBService.Core.UnitTest;

public class UtensilRepositoryUnitTest
{
    [Fact]
    public async Task Get_Brand_GameProvider_Configuration_Id()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        var brandId = Guid.NewGuid();
        var gameProviderId = Guid.NewGuid();

        var key = $"gamerecord:{gameRecordInfo.Value.GameProviderCode}:{brandId}:{gameProviderId}";

        _ = fakeMemoryCache
            .TryGetValue(
                key: Arg.Is(key),
                value: out _)
             .Returns(false);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeClient
            .BrandGetGameProviderConfigurationIdAsync(
                brandId: Arg.Is(brandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = await repository
            .GetBrandGameProviderConfigurationIdAsync(
                brandId,
                gameProviderId)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.HasValue);
        Assert.Equal(gameProviderConfigurationId, actual.Value);

        _ = fakeMemoryCache
            .Received(1)
            .Set(
                key: key,
                gameProviderConfigurationId);
    }

    [Fact]
    public async Task Get_Game_Id()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        var gameProviderId = Guid.NewGuid();
        var gameExternalId = "1";

        var key = $"gamerecord:{gameRecordInfo.Value.GameProviderCode}:{gameProviderId}:{gameExternalId}";

        _ = fakeMemoryCache
            .TryGetValue(
                key: Arg.Is(key),
                value: out _)
             .Returns(false);

        var gameId = 1;
        _ = fakeClient
            .GameGetIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(gameExternalId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = await repository
            .GetGameIdAsync(
                gameProviderId,
                gameExternalId)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.HasValue);
        Assert.Equal(gameId, actual.Value);

        _ = fakeMemoryCache
            .Received(1)
            .Set(
                key: key,
                gameId);
    }

    [Fact]
    public async Task Get_GameProvider_Id()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        var key = $"gamerecord:{gameRecordInfo.Value.GameProviderCode}";

        _ = fakeMemoryCache
            .TryGetValue(
                key: Arg.Is(key),
                value: out _)
             .Returns(false);

        var gameProviderId = Guid.NewGuid();
        _ = fakeClient
            .GameProviderGetGameProviderIdAsync(
                gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = await repository
            .GetGameProviderIdAsync()
            .ConfigureAwait(false);

        Assert.Equal(gameProviderId, actual);

        _ = fakeMemoryCache
            .Received(1)
            .Set(
                key: key,
                gameProviderId);
    }

    [Fact]
    public async Task Get_GameProvider_Id_Exception()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        var key = $"{gameRecordInfo.Value.GameProviderCode}";

        _ = fakeMemoryCache
            .TryGetValue(
                key: Arg.Is(key),
                value: out _)
             .Returns(false);

        _ = fakeClient
            .GameProviderGetGameProviderIdAsync(
                gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<Guid?>(null));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        _ = await Assert.ThrowsAsync<Exception>(() => repository
             .GetGameProviderIdAsync()
             .AsTask())
             .ConfigureAwait(false);
    }

    [Fact]
    public async Task Get_Player()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        var playerCode = "test";

        var key = $"gamerecord:{gameRecordInfo.Value.GameProviderCode}:{playerCode}";

        _ = fakeMemoryCache
            .TryGetValue(
                key: Arg.Is(key),
                value: out _)
             .Returns(false);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "GPI",
            PlayerCode: playerCode,
            Username: playerCode,
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "CNY");
        _ = fakeClient
            .PlayerGetAsync(
                playerCode: Arg.Is(playerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = await repository
            .GetPlayerAsync(
                playerCode: playerCode)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(playerCode, player.PlayerCode);

        _ = fakeMemoryCache
            .Received(1)
            .Set(
                key: key,
                player);
    }
    [Fact]
    public void Time_Difference_Within_2_Hours_Less_Than_10_Seconds_True()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T12:59:50+08:00"));

        Assert.True(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_Less_Than_2_Hours_True()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T11:00:00+08:00"));

        Assert.True(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_Less_Than_20_Minutes_True()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T12:40:00+08:00"));

        Assert.True(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_More_Than_10_Seconds_False()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T13:00:10+08:00"));

        Assert.False(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_More_Than_2_Hours_False()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T15:00:00+08:00"));

        Assert.False(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_More_Than_20_Minutes_False()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameCommandClient>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var absoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            gameRecordInfo,
            fakeClient,
            fakeMemoryCache,
            fakeSystemClock,
            absoluteExpirationRelativeToNow);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T13:20:00+08:00"));

        Assert.False(actual);
    }
}
