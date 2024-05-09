using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameRecord.JDB.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDB.Core.UnitTest.ApplicationServices;

public class ManualPullRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process_TimeDifferenceWithin2Hours_False()
	{
		var fakeLogger = NullLogger<ManualPullRecordCommandService>.Instance;
		var fakeClient = Substitute.For<IGameInterfaceJDBClient>();
		var fakeUtensil = Substitute.For<IUtensil>();
		var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

		var command = new ManualPullRecordCommand(
			ApiUrl: new("http://localhost/"),
			Domain: "ZF",
			AesKey: "193a3159afebee65",
			AesIV: "831057181dd07807",
			Agent: "zf055gpi",
			StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
			EndTime: DateTimeOffset.UtcNow);

		_ = fakeUtensil
			.TimeDifferenceWithin2Hours(
				source: Arg.Is(command.EndTime))
			.Returns(false);

		var service = new ManualPullRecordCommandService(
			fakeLogger,
			fakeClient,
			fakeUtensil,
			fakeService);

		await service.ExecuteAsync(command: command);

		_ = await fakeClient
			.Received(0)
			.GameWagerAsync(
				apiUrl: Arg.Is(command.ApiUrl),
				domain: Arg.Is(command.Domain),
				aesKey: Arg.Is(command.AesKey),
				aesIV: Arg.Is(command.AesIV),
				agent: Arg.Is(command.Agent),
				startTime: Arg.Is(command.StartTime.UtcDateTime),
				endTime: Arg.Is(command.EndTime.UtcDateTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.ToArrayAsync();

		_ = await fakeClient
			.Received(1)
			.GameWagerOldAsync(
				apiUrl: Arg.Is(command.ApiUrl),
				domain: Arg.Is(command.Domain),
				aesKey: Arg.Is(command.AesKey),
				aesIV: Arg.Is(command.AesIV),
				agent: Arg.Is(command.Agent),
				startTime: Arg.Is(command.StartTime.UtcDateTime),
				endTime: Arg.Is(command.EndTime.UtcDateTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.ToArrayAsync();
	}

	[Fact]
	public async Task Normal_Process_TimeDifferenceWithin2Hours_True()
	{
		var fakeLogger = NullLogger<ManualPullRecordCommandService>.Instance;
		var fakeClient = Substitute.For<IGameInterfaceJDBClient>();
		var fakeUtensil = Substitute.For<IUtensil>();
		var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

		var command = new ManualPullRecordCommand(
			ApiUrl: new("http://localhost/"),
			Domain: "ZF",
			AesKey: "193a3159afebee65",
			AesIV: "831057181dd07807",
			Agent: "zf055gpi",
			StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
			EndTime: DateTimeOffset.UtcNow);

		_ = fakeUtensil
			.TimeDifferenceWithin2Hours(
				source: Arg.Is(command.EndTime))
			.Returns(true);

		var service = new ManualPullRecordCommandService(
			fakeLogger,
			fakeClient,
			fakeUtensil,
			fakeService);

		await service.ExecuteAsync(command: command);

		_ = await fakeClient
			.Received(1)
			.GameWagerAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			domain: Arg.Is(command.Domain),
			aesKey: Arg.Is(command.AesKey),
			aesIV: Arg.Is(command.AesIV),
			agent: Arg.Is(command.Agent),
			startTime: Arg.Is(command.StartTime.UtcDateTime),
			endTime: Arg.Is(command.EndTime.UtcDateTime),
			cancellationToken: Arg.Any<CancellationToken>())
			.ToArrayAsync();

		_ = await fakeClient
			.Received(0)
			.GameWagerOldAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			domain: Arg.Is(command.Domain),
			aesKey: Arg.Is(command.AesKey),
			aesIV: Arg.Is(command.AesIV),
			agent: Arg.Is(command.Agent),
			startTime: Arg.Is(command.StartTime.UtcDateTime),
			endTime: Arg.Is(command.EndTime.UtcDateTime),
			cancellationToken: Arg.Any<CancellationToken>())
			.ToArrayAsync();
	}
}
