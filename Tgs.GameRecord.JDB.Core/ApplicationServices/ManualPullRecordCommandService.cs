using Microsoft.Extensions.Logging;
using Tgs.GameInterface.JDBService.Client;
using Tgs.GameInterface.JDBService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDB.Core.ApplicationServices;

internal class ManualPullRecordCommandService(
	ILogger<ManualPullRecordCommandService> logger,
	IGameInterfaceJDBClient client,
	IUtensil utensil,
	ICommandService<HandleRecordCommand> service) : ICommandService<ManualPullRecordCommand>
{
	public async ValueTask ExecuteAsync(ManualPullRecordCommand command, CancellationToken cancellationToken = default)
	{
		var gameRecords = utensil.TimeDifferenceWithin2Hours(command.EndTime)
			? await client
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
			: await client
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

		logger.LogDebug("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

		foreach (var gameRecord in gameRecords)
		{
			logger.LogDebug("LogOn:{logOn} Message: Settlement. GameRecord:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

			try
			{
				await service
					.ExecuteAsync(
					command: new(gameRecord),
					cancellationToken: cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "LogOn:{logOn} Message: An exception occurred in this game record.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
			}
		}
	}
}
