using Microsoft.Extensions.Logging;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.GameRecord.JDBService.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDBService.Core.WorkerServices;

internal class ScheduleWorkerService : ICommandService<ScheduleWorkerCommand>
{
    private readonly IGameInterfaceJDBClient _client;
    private readonly ILogger<ScheduleWorkerService> _logger;
    private readonly ICommandService<GamePullRecordCommand> _service;

    public ScheduleWorkerService(
        IGameInterfaceJDBClient client,
        ILogger<ScheduleWorkerService> logger,
        ICommandService<GamePullRecordCommand> service)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(ScheduleWorkerCommand command, CancellationToken cancellationToken = default)
    {
        var gameHistoryInfos = await _client
                .GameHistoryAsync(
                    apiUrl: command.ApiUrl,
                    domain: command.Domain,
                    aesKey: command.AesKey,
                    aesIV: command.AesIV,
                    agent: command.Agent,
                    startTime: command.StartTime,
                    endTime: command.EndTime,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<GameHistoryInfo>();

        _logger.LogInformation("LogOn:{logOn} | Request:{request} | Response:{response}",
            DateTime.UtcNow.ToString("O"),
            command,
            gameHistoryInfos);

        if (!gameHistoryInfos.Any())
        {
            return;
        }

        await _service.ExecuteAsync(new GamePullRecordCommand(
                       Records: gameHistoryInfos), cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
