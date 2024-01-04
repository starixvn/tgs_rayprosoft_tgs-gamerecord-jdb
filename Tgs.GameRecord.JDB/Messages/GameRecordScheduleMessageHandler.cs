using System.Text;
using System.Text.Json;
using Tgs.GameRecord.JDB.Common;
using Tgs.GameRecord.JDB.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.JDB.Messages;

public class GameRecordScheduleMessageHandler(
    ILogger<GameRecordScheduleMessageHandler> logger,
    ICommandService<SchedulePullRecordCommand> service) : IMessageHandler
{
    public ValueTask HandleAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        var json = Encoding.UTF8.GetString(data.Span);
        var message = JsonSerializer.Deserialize<JDBGameRecordMessage>(json)
            ?? throw new NullReferenceException("The request data resolves to a null value");

        logger.LogInformation("LogOn:{logOn} Information Request:{message}", DateTime.Now.ToString("o"), message);

        var command = new SchedulePullRecordCommand(
            ApiUrl: message.ApiUrl,
            Domain: message.Domain,
            AesKey: message.AesKey,
            AesIV: message.AesIV,
            Agent: message.Agent,
            StartTime: message.StartTime,
            EndTime: message.EndTime);

        return service.ExecuteAsync(
            command: command,
            cancellationToken: cancellationToken);
    }
}
