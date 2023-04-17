using Tgs.GameInterface.JDBService.Protos;
using Tgs.GameRecord.JDBService.Core.Models;

namespace Tgs.GameRecord.JDBService.Core;

public interface IGameRecordUtensil
{
    public ValueTask<RecordInfo> SettlementConvertRecordAsync(
        GameHistoryInfo source,
        CancellationToken cancellationToken = default);
}
