using Tgs.GameInterface.JDBService.Protos;
using Tgs.GameRecord.JDBService.Core.Models;

namespace Tgs.GameRecord.JDBService.Core;

internal class GameRecordUtensil : IGameRecordUtensil
{
    private readonly IUtensil _utensil;

    public GameRecordUtensil(
        IUtensil utensil)
    {
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
    }

    public async ValueTask<RecordInfo> SettlementConvertRecordAsync(
        GameHistoryInfo source,
        CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _utensil
            .GetGameProviderIdAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameProviderCode = _utensil
            .GetGameProviderCode();

        var player = await _utensil
            .GetPlayerAsync(
                playerCode: source.PlayerCode,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var gameId = await _utensil
            .GetGameIdAsync(
                gameProviderId: gameProviderId,
                gameExternalId: source.ExternalGameId.ToString(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameProviderConfigurationId = await _utensil
            .GetBrandGameProviderConfigurationIdAsync(
            brandId: player.BrandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var externalCreatedOn = source.ExternalCreatedOn.ToDateTimeOffset();
        var externalClosedOn = source.ExternalClosedOn.ToDateTimeOffset();
        var bet = source.Bet.ToDecimal().GetValueOrDefault(0);
        var validBet = source.ValidBet.ToDecimal().GetValueOrDefault(0);
        var turnover = source.Turnover.ToDecimal().GetValueOrDefault(0);
        var winLoss = source.PlayerWinLoss.ToDecimal().GetValueOrDefault(0);
        var settle = source.Settle.ToDecimal().GetValueOrDefault(0);
        var commission = 0;

        return new RecordInfo(
            GameProviderId: gameProviderId,
            GameProviderCode: gameProviderCode,
            ExternalRoundId: source.ExternalRoundId,
            GameId: gameId,
            ExternalGameId: source.ExternalGameId.ToString(),
            PlayerCode: player.PlayerCode,
            WalletId: player.DefaultWalletId,
            CurrencyCode: player.DefaultWalletCurrencyCode,
            BrandId: player.BrandId,
            GameProviderConfigurationId: gameProviderConfigurationId,
            ExternalCreatedOn: externalCreatedOn,
            ExternalClosedOn: externalClosedOn,
            Bet: bet,
            ValidBet: validBet,
            Settle: settle,
            Commission: commission,
            Turnover: turnover,
            WinLoss: winLoss,
            AdditionalData: source.AdditionalData);
    }
}
