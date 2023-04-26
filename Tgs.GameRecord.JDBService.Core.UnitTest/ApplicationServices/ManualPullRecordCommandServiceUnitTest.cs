using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameRecord.JDBService.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDBService.Core.UnitTest.ApplicationServices;

public class ManualPullRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<ManualPullRecordCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceJDBClient>();
        var fakeGameRecordUtensil = Substitute.For<IGameRecordUtensil>();
        var fakeService = Substitute.For<ICommandService<ManualHandleRecordCommand>>();

        var command = new ManualPullRecordCommand(
            ApiUrl: new("http://localhost/"),
            Domain: "ZF",
            AesKey: "193a3159afebee65",
            AesIV: "831057181dd07807",
            Agent: "zf055gpi",
            StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow);

        var service = new ManualPullRecordCommandService(
            fakeLogger,
            fakeClient,
            fakeGameRecordUtensil,
            fakeService);

        await service
            .ExecuteAsync(
                command: command)
            .ConfigureAwait(false);

        _ = await fakeClient
            .Received(1)
            .GameOldHistoryAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                domain: Arg.Is(command.Domain),
                aesKey: Arg.Is(command.AesKey),
                aesIV: Arg.Is(command.AesIV),
                agent: Arg.Is(command.Agent),
                startTime: Arg.Is(command.StartTime.UtcDateTime),
                endTime: Arg.Is(command.EndTime.UtcDateTime),
                cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync()
            .ConfigureAwait(false);
    }
}
