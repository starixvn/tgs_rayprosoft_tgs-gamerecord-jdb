using Microsoft.Extensions.Logging;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDB.Core.ApplicationServices;

internal class ManualPullRecordCommandService : ICommandService<ManualPullRecordCommand>
{
    private readonly IGameInterfaceJDBClient _client;
    private readonly ILogger<ManualPullRecordCommandService> _logger;
    private readonly ICommandService<HandleRecordCommand> _service;
    private readonly IUtensil _utensil;

    public ManualPullRecordCommandService(
        ILogger<ManualPullRecordCommandService> logger,
        IGameInterfaceJDBClient client,
        IUtensil utensil,
        ICommandService<HandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(ManualPullRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameRecords = _utensil.TimeDifferenceWithin2Hours(command.EndTime)
            ? await _client
            .GameWagerAsync(
                apiUrl: command.ApiUrl,
                domain: command.Domain,
                aesKey: command.AesKey,
                aesIV: command.AesIV,
                agent: command.Agent,
                startTime: command.StartTime.UtcDateTime,
                endTime: command.EndTime.UtcDateTime,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken).ConfigureAwait(false)
            ?? Enumerable.Empty<GameWagerInfo>()
            : await _client
            .GameWagerOldAsync(
                apiUrl: command.ApiUrl,
                domain: command.Domain,
                aesKey: command.AesKey,
                aesIV: command.AesIV,
                agent: command.Agent,
                startTime: command.StartTime.UtcDateTime,
                endTime: command.EndTime.UtcDateTime,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken).ConfigureAwait(false)
            ?? Enumerable.Empty<GameWagerInfo>();

        _logger.LogDebug("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

        foreach (var gameRecord in gameRecords)
        {
            _logger.LogDebug("LogOn:{logOn} Message: Settlement. GameRecord:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

            try
            {
                await _service
                    .ExecuteAsync(
                    command: new(gameRecord),
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogOn:{logOn} Message: An exception occurred in this game record.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
            }
        }
    }
}
