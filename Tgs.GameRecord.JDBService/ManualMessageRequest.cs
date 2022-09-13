namespace Tgs.GameRecord.JDBService;

public record ManualMessageRequest(
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
