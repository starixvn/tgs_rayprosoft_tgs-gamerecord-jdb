using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameInterface.JDBService.Protos;

namespace Tgs.GameRecord.JDBService.Core.UnitTest;

public class GameRecordUtensilUnitTest
{
    [Fact]
    public async Task Settlement_Normal_Process()
    {
        var fakeUtensil = Substitute.For<IUtensil>();

        var source = new GameHistoryInfo
        {
            ExternalRoundId = "7371097809797",
            ExternalGameId = "14027",
            PlayerCode = "4ujlqxs6wd9f",
            ExternalCreatedOn = DateTimeOffset.Parse("2023-04-16T11:55:57-04:00").ToTimestamp(),
            ExternalClosedOn = DateTimeOffset.Parse("2023-04-16T11:55:57-04:00").ToTimestamp(),
            Bet = 1.8m,
            ValidBet = 1.8m,
            Settle = 0,
            Turnover = 0,
            PlayerWinLoss = -1.8m,
            AdditionalData = "{\"seqNo\":7371097809797,\"playerId\":\"4ujlqxs6wd9f\",\"gType\":0,\"mtype\":14027,\"gameDate\":\"2023-04-16T11:55:57-04:00\",\"bet\":-1.8,\"win\":0,\"total\":-1.8,\"currency\":\"PP\",\"jackpot\":0,\"jackpotContribute\":0,\"denom\":0.2,\"lastModifyTime\":\"2023-04-16T11:55:57-04:00\",\"playerIp\":\"158.62.0.22\",\"clientType\":\"MOBILE\",\"hasFreegame\":0,\"hasGamble\":0,\"gambleBet\":0,\"systemTakeWin\":0}"
        };

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "JDBTF";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "PB2",
            PlayerCode: source.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(source.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(source.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var gameRecordUtensil = new GameRecordUtensil(fakeUtensil);

        var actual = await gameRecordUtensil
             .SettlementConvertRecordAsync(
                 source: source)
             .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameProviderCode, actual.GameProviderCode);
        Assert.Equal(source.ExternalRoundId, actual.ExternalRoundId);
        Assert.Equal(gameId, actual.GameId);
        Assert.Equal(source.ExternalGameId.ToString(), actual.ExternalGameId);
        Assert.Equal(source.PlayerCode, actual.PlayerCode);
        Assert.Equal(player.DefaultWalletId, actual.WalletId);
        Assert.Equal(player.DefaultWalletCurrencyCode, actual.CurrencyCode);
        Assert.Equal(player.BrandId, actual.BrandId);
        Assert.Equal(gameProviderConfigurationId, actual.GameProviderConfigurationId);
        Assert.Equal(source.ExternalCreatedOn.ToDateTimeOffset(), actual.ExternalCreatedOn);
        Assert.Equal(source.ExternalClosedOn.ToDateTimeOffset(), actual.ExternalClosedOn);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Bet);
        Assert.Equal(source.ValidBet.ToDecimal().GetValueOrDefault(0), actual.ValidBet);
        Assert.Equal(source.Turnover.ToDecimal().GetValueOrDefault(0), actual.Turnover);
        Assert.Equal(source.PlayerWinLoss.ToDecimal().GetValueOrDefault(0), actual.WinLoss);
        Assert.Equal(source.Settle.ToDecimal().GetValueOrDefault(0), actual.Settle);
        Assert.Equal(source.AdditionalData, actual.AdditionalData);
    }
}
