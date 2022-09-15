namespace Tgs.GameRecord.JDBService;

public record ScheduleMessageModel(
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent);
