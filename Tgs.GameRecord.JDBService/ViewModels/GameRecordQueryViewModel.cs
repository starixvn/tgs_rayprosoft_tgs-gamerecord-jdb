namespace Tgs.GameRecord.JDBService.ViewModels;

public record GameRecordQueryViewModel(
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
