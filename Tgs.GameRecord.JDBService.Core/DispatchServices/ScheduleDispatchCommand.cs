namespace Tgs.GameRecord.JDBService.Core.DispatchServices;

public record ScheduleDispatchCommand(
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent);
