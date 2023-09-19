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
            Record: new GameWagerInfo
            {
                ExternalWagerId = "7383541906849",
                ExternalGameId = "0_14065",
                PlayerCode = "4uhm33dvljda",
                FreeGameState = false,
                GambleState = false,
                ExternalCreatedOn = DateTimeOffset.Parse("2023-09-18T01:38:40-04:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-09-18T01:38:40-04:00").ToTimestamp(),
                Bet = 5.0m,
                Settle = 0.0m,
                WinLoss = -5.0m,
                AdditionalData = "{\"seqNo\":7383541906849,\"playerId\":\"4uhm33dvljda\",\"gType\":0,\"mtype\":14065,\"gameDate\":\"18-09-2023 01:38:40\",\"bet\":-5,\"win\":0,\"total\":-5,\"currency\":\"PP\",\"jackpot\":0,\"jackpotContribute\":0,\"denom\":0.1,\"lastModifyTime\":\"18-09-2023 01:38:40\",\"playerIp\":\"131.226.106.180\",\"clientType\":\"MOBILE\",\"hasFreegame\":0,\"hasGamble\":0,\"gambleBet\":0,\"systemTakeWin\":0}\r\n"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
            command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Lose),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalWagerId),
            externalWagerId: Arg.Is(command.Record.ExternalWagerId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.WinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
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
            Record: new GameWagerInfo
            {
                ExternalWagerId = "7383541906849",
                ExternalGameId = "0_14065",
                PlayerCode = "4uhm33dvljda",
                FreeGameState = false,
                GambleState = false,
                ExternalCreatedOn = DateTimeOffset.Parse("2023-09-18T01:38:40-04:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-09-18T01:38:40-04:00").ToTimestamp(),
                Bet = 5.0m,
                Settle = 10.0m,
                WinLoss = 5.0m,
                AdditionalData = "{\"seqNo\":7383541906849,\"playerId\":\"4uhm33dvljda\",\"gType\":0,\"mtype\":14065,\"gameDate\":\"18-09-2023 01:38:40\",\"bet\":-5,\"win\":10,\"total\":5,\"currency\":\"PP\",\"jackpot\":0,\"jackpotContribute\":0,\"denom\":0.1,\"lastModifyTime\":\"18-09-2023 01:38:40\",\"playerIp\":\"131.226.106.180\",\"clientType\":\"MOBILE\",\"hasFreegame\":0,\"hasGamble\":0,\"gambleBet\":0,\"systemTakeWin\":0}\r\n"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
            command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Win),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalWagerId),
            externalWagerId: Arg.Is(command.Record.ExternalWagerId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.WinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
