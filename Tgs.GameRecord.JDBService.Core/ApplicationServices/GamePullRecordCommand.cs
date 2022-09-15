using Tgs.GameInterface.JDBService.Protos;

namespace Tgs.GameRecord.JDBService.Core.ApplicationServices;

public record GamePullRecordCommand(IEnumerable<GameHistoryInfo> Records);
