using Microsoft.Extensions.Options;
using Tgs.GameRecord.JDBService.Core.WorkerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDBService.Core.DispatchServices;

internal class ScheduleDispatchService : ICommandService<ScheduleDispatchCommand>
{
    private readonly ICommandService<ScheduleWorkerCommand> _commandService;
    private readonly GameRecordInfoOptions _gameRecordInfoOptions;
    private readonly double _intervalMinute;
    private readonly IScheduleDispatch _scheduleDispatch;

    public ScheduleDispatchService(
        IOptions<GameRecordInfoOptions> options,
        ICommandService<ScheduleWorkerCommand> commandService,
        IScheduleDispatch scheduleDispatch
        )
    {
        _gameRecordInfoOptions = options.Value;
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _scheduleDispatch = scheduleDispatch ?? throw new ArgumentNullException(nameof(scheduleDispatch));
        _intervalMinute = (_gameRecordInfoOptions.IntervalMinute > _gameRecordInfoOptions.MaxIntervalMinute)
                        ? _gameRecordInfoOptions.MaxIntervalMinute
                        : (_gameRecordInfoOptions.IntervalMinute < _gameRecordInfoOptions.MinIntervalMinute)
                        ? _gameRecordInfoOptions.MinIntervalMinute
                        : _gameRecordInfoOptions.IntervalMinute;
    }

    public async ValueTask ExecuteAsync(ScheduleDispatchCommand command, CancellationToken cancellationToken = default)
    {
        var key = $"gp:gamerecord:{_gameRecordInfoOptions.GameProviderCode}:{command.Domain}:{command.Agent}".ToLower();
        if (!await _scheduleDispatch.ExistAsync(key: key).ConfigureAwait(false))
        {
            if (await _scheduleDispatch.LockingAsync(key: key, command: command).ConfigureAwait(false))
            {
                try
                {
                    var endTime = DateTime.Parse(DateTime.UtcNow.ToString(@"yyyy-MM-dd\THH:mm:00K"));

                    var startTime = endTime.AddMinutes(_intervalMinute * -1);

                    var workerCommand = new ScheduleWorkerCommand(
                         ApiUrl: command.ApiUrl,
                         Domain: command.Domain,
                         AesKey: command.AesKey,
                         AesIV: command.AesIV,
                         Agent: command.Agent,
                         StartTime: startTime,
                         EndTime: endTime);

                    await _commandService
                            .ExecuteAsync(
                                command: workerCommand,
                                cancellationToken: cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    await _scheduleDispatch.UnlockAsync(key: key).ConfigureAwait(false);
                }
            }
        }
    }
}
