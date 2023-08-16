using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.GameRecord.JDBService.Core.ApplicationServices;

namespace Tgs.GameRecord.JDBService.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async void Normal_Process_Settlement_Loss()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            Record: new GameHistoryInfo
            {
                ExternalRoundId = "7371097809797",
                ExternalGameId = "14027",
                PlayerCode = "4ujlqxs6wd9f",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-04-16T11:55:57-04:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-04-16T11:55:57-04:00").ToTimestamp(),
                Bet = 1.8m,
                ValidBet = 1.8m,
                Settle = 0,
                Turnover = 1.8m,
                PlayerWinLoss = -1.8m,
                AdditionalData = "{\"seqNo\":7371097809797,\"playerId\":\"4ujlqxs6wd9f\",\"gType\":0,\"mtype\":14027,\"gameDate\":\"2023-04-16T11:55:57-04:00\",\"bet\":-1.8,\"win\":0,\"total\":-1.8,\"currency\":\"PP\",\"jackpot\":0,\"jackpotContribute\":0,\"denom\":0.2,\"lastModifyTime\":\"2023-04-16T11:55:57-04:00\",\"playerIp\":\"158.62.0.22\",\"clientType\":\"MOBILE\",\"hasFreegame\":0,\"hasGamble\":0,\"gambleBet\":0,\"systemTakeWin\":0}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .MessageAsync(
                gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(command.Record.PlayerCode),
                status: Arg.Is(GameRecordStatus.Closed),
                clientStatus: Arg.Is(GameRecordClientStatus.Loss),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.ValidBet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(0.0m),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Turnover.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async void Normal_Process_Settlement_Win()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "JDBTF"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            Record: new GameHistoryInfo
            {
                ExternalRoundId = "7371097809797",
                ExternalGameId = "14027",
                PlayerCode = "4ujlqxs6wd9f",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-04-16T11:55:57-04:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-04-16T11:55:57-04:00").ToTimestamp(),
                Bet = 1.8m,
                ValidBet = 1.8m,
                Settle = 3.6m,
                Turnover = 1.8m,
                PlayerWinLoss = 1.8m,
                AdditionalData = "{\"seqNo\":7371097809797,\"playerId\":\"4ujlqxs6wd9f\",\"gType\":0,\"mtype\":14027,\"gameDate\":\"2023-04-16T11:55:57-04:00\",\"bet\":-1.8,\"win\":0,\"total\":-1.8,\"currency\":\"PP\",\"jackpot\":0,\"jackpotContribute\":0,\"denom\":0.2,\"lastModifyTime\":\"2023-04-16T11:55:57-04:00\",\"playerIp\":\"158.62.0.22\",\"clientType\":\"MOBILE\",\"hasFreegame\":0,\"hasGamble\":0,\"gambleBet\":0,\"systemTakeWin\":0}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .MessageAsync(
                gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(command.Record.PlayerCode),
                status: Arg.Is(GameRecordStatus.Closed),
                clientStatus: Arg.Is(GameRecordClientStatus.Win),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.ValidBet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Turnover.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
