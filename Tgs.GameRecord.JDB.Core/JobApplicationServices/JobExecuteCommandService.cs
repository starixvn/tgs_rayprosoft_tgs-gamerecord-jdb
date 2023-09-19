using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.GameRounds.Schedule.Client;
using Tgs.GameRounds.Schedule.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDB.Core.JobApplicationServices;

internal class JobExecuteCommandService : ICommandService<JobExecuteCommand>
{
    private readonly IGameRoundsScheduleClient _client;
    private readonly GameRecordInfoOptions _gameRecordInfo;
    private readonly double _intervalMinutes;
    private readonly ILogger<JobExecuteCommandService> _logger;

    public JobExecuteCommandService(
        ILogger<JobExecuteCommandService> logger,
        IOptions<GameRecordInfoOptions> options,
        IGameRoundsScheduleClient client)
    {
        _gameRecordInfo = options.Value;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));

        _intervalMinutes = _gameRecordInfo.IntervalMinutesMax < _gameRecordInfo.IntervalMinutes
            ? _gameRecordInfo.IntervalMinutesMax
            : _gameRecordInfo.IntervalMinutesMinimum > _gameRecordInfo.IntervalMinutes
            ? _gameRecordInfo.IntervalMinutesMinimum
            : _gameRecordInfo.IntervalMinutes;
    }

    public ValueTask ExecuteAsync(JobExecuteCommand command, CancellationToken cancellationToken = default)
    {
        //每次查詢時間範圍最多為 15 分鐘，最新信息為 3 分鐘前，當前時間小於3分鐘會廠商會出現錯誤8006
        //當前時間22/09/12 14:15
        //查詢時間22/09/12 14:15~14:00會回應{"status":"8006","err_text":"Time does not meet."}
        //查詢時間22/09/12 14:12~13:56會回應{"status":"8009","err_text":"Time interval exceeds the allowable range."}
        //查詢時間22/09/12 14:12~13:57會回應{"status":"0000","err_text":"","data:":"[]"}
        var endTime = DateTimeOffset.ParseExact(DateTimeOffset.UtcNow.AddMinutes(-3).ToString(@"yyyy-MM-dd\THH:mm:00K"), @"yyyy-MM-dd\THH:mm:ssK", null, styles: System.Globalization.DateTimeStyles.RoundtripKind);

        var startTime = endTime.AddMinutes(_intervalMinutes * -1);
        var parameters = new
        {
            command.GameProviderCode,
            command.ApiUrl,
            command.Domain,
            command.AesKey,
            command.AesIV,
            command.Agent,
            StartTime = startTime,
            EndTime = endTime
        };

        _logger.LogDebug("LogOn:{logOn} Parameters:{parameters}", DateTime.Now.ToString("u"), parameters);

        return _client
            .JDB_ScheduleFormQueuedAsync(
                gameProviderCode: command.GameProviderCode,
                name: command.Name,
                subject: "gp.gamerecord.scheduleform.jdbtf.queued",
                source: ScheduleFormSource.Schedule,
                apiUrl: parameters.ApiUrl,
                domain: parameters.Domain,
                aesKey: parameters.AesKey,
                aesIV: parameters.AesIV,
                agent: parameters.Agent,
                startTime: parameters.StartTime,
                endTime: parameters.EndTime,
                cancellationToken: cancellationToken);
    }
}
