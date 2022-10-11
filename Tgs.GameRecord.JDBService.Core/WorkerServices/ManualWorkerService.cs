using Microsoft.Extensions.Logging;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.GameRecord.JDBService.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDBService.Core.WorkerServices;

internal class ManualWorkerService : ICommandService<ManualWorkerCommand>
{
    private readonly IGameInterfaceJDBClient _client;
    private readonly ILogger<ManualWorkerService> _logger;
    private readonly ICommandService<GamePullRecordCommand> _service;

    public ManualWorkerService(
        IGameInterfaceJDBClient client,
        ILogger<ManualWorkerService> logger,
        ICommandService<GamePullRecordCommand> service)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(ManualWorkerCommand command, CancellationToken cancellationToken = default)
    {
        //每次查詢時間範圍最多為 15 分鐘，最新信息為 3 分鐘前，當前時間小於3分鐘會廠商會出現錯誤8006
        //當前時間22/09/12 14:15
        //查詢時間22/09/12 14:15~14:00會回應{"status":"8006","err_text":"Time does not meet."}
        //查詢時間22/09/12 14:12~13:56會回應{"status":"8009","err_text":"Time interval exceeds the allowable range."}
        //查詢時間22/09/12 14:12~13:57會回應{"status":"0000","err_text":"","data:":"[]"}
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

        if (gameHistoryInfos.Any())
        {
            await _service.ExecuteAsync(new GamePullRecordCommand(
                Records: gameHistoryInfos), cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        //每次查詢時間範圍最多為 5 分鐘。提供超過 2 小時至 60 天內交易信息。
        //當前時間22/09/12 14:15
        //查詢時間22/09/12 14:15~14:00會回應{"status":"8006","err_text":"Time does not meet."}
        //查詢時間22/09/12 12:09~12:15會回應{"status":"8009","err_text":"Time interval exceeds the allowable range."}
        //查詢時間22/09/12 12:10~12:15會回應{"status":"0000","err_text":"","data:":"[]"}
        gameHistoryInfos = await _client
                .GameOldHistoryAsync(
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

        if (gameHistoryInfos.Any())
        {
            await _service.ExecuteAsync(new GamePullRecordCommand(
                Records: gameHistoryInfos), cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
