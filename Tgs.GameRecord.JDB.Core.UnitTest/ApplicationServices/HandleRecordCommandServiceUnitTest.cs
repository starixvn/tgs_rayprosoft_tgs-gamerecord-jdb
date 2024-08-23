using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.JDB.Core.ApplicationServices;

namespace Tgs.GameRecord.JDB.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Freegame()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "JDBTF",
			GameProviderConfiguration = "JDBTFConfiguration",
			HangfirePrefix = "gamerecord-jdbtf-hangfire:",
			HangfireLocking = "gamerecord-jdbtf-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 5,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Record: new GameWagerInfo
			{
				ExternalRoundId = "5250214464421",
				ExternalWagerId = "5250214464421",
				ExternalGameId = "0_15010",
				PlayerCode = "4ugse0rsnufp",
				FreeGameState = true,
				GambleState = false,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-09-19T23:38:01-04:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-09-19T23:38:01-04:00").ToTimestamp(),
				Bet = 0.5m,
				Settle = 65.5m,
				WinLoss = 65.0m,
				AdditionalData = new JsonObject
				{
					["seqNo"] = 5250214464421,
					["playerId"] = "4ugse0rsnufp",
					["gType"] = 0,
					["mtype"] = 15010,
					["gameDate"] = "19-09-2023 23:38:01",
					["bet"] = -0.5,
					["win"] = 65.5,
					["total"] = 65.0,
					["currency"] = "RB",
					["jackpot"] = 0.0,
					["jackpotContribute"] = -0.0025,
					["denom"] = 0.01,
					["lastModifyTime"] = "19-09-2023 23:38:01",
					["playerIp"] = "114.33.41.237",
					["clientType"] = "WEB",
					["hasFreegame"] = 1,
					["hasGamble"] = 0,
					["gambleBet"] = 0.0,
					["systemTakeWin"] = 0
				}.ToJsonString()
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalWagerId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.FreeGame,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 0.5m,
				ValidBet = 0.5m,
				Settle = 65.5m,
				Commission = 0.0M,
				Turnover = 0.5m,
				WinLoss = 65.0m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "JDBTF",
			GameProviderConfiguration = "JDBTFConfiguration",
			HangfirePrefix = "gamerecord-jdbtf-hangfire:",
			HangfireLocking = "gamerecord-jdbtf-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 5,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Record: new GameWagerInfo
			{
				ExternalRoundId = "7383541906849",
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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalWagerId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 5.0m,
				ValidBet = 5.0m,
				Settle = 0.0m,
				Commission = 0.0M,
				Turnover = 5.0m,
				WinLoss = -5.0m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "JDBTF",
			GameProviderConfiguration = "JDBTFConfiguration",
			HangfirePrefix = "gamerecord-jdbtf-hangfire:",
			HangfireLocking = "gamerecord-jdbtf-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 5,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Record: new GameWagerInfo
			{
				ExternalRoundId = "7383541906849",
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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalWagerId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 5.0m,
				ValidBet = 5.0m,
				Settle = 10.0m,
				Commission = 0.0M,
				Turnover = 5.0m,
				WinLoss = 5.0m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
