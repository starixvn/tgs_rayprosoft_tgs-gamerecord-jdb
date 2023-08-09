using Microsoft.Extensions.Logging;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDBService.Core.ApplicationServices;

internal class SchedulePullRecordCommandService : ICommandService<SchedulePullRecordCommand>
{
    private readonly IGameInterfaceJDBClient _client;
    private readonly IGameRecordUtensil _gameRecordUtensil;
    private readonly ILogger<SchedulePullRecordCommandService> _logger;
    private readonly ICommandService<HandleRecordCommand> _service;

    public SchedulePullRecordCommandService(
        ILogger<SchedulePullRecordCommandService> logger,
        IGameInterfaceJDBClient client,
        IGameRecordUtensil gameRecordUtensil,
        ICommandService<HandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _gameRecordUtensil = gameRecordUtensil ?? throw new ArgumentNullException(nameof(gameRecordUtensil));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(SchedulePullRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameRecords = await _client
            .GameHistoryAsync(
                apiUrl: command.ApiUrl,
                domain: command.Domain,
                aesKey: command.AesKey,
                aesIV: command.AesIV,
                agent: command.Agent,
                startTime: command.StartTime.UtcDateTime,
                endTime: command.EndTime.UtcDateTime,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken).ConfigureAwait(false)
            ?? Enumerable.Empty<GameHistoryInfo>();

        _logger.LogDebug("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

        foreach (var gameRecord in gameRecords)
        {
            _logger.LogDebug("LogOn:{logOn} Message: Settlement. GameRecord:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

            try
            {
                var record = await _gameRecordUtensil
                    .SettlementConvertRecordAsync(
                        source: gameRecord,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                _logger.LogDebug("LogOn:{logOn} Message: Settlement. Record:{record}", DateTime.Now.ToString("o"), record);

                await _service
                    .ExecuteAsync(
                        command: new(record),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogOn:{logOn} Message: Settlement Exception. GameRecord:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);
            }
        }
    }
}
