namespace Tgs.GameRecord.JDBManualService.ViewModels;

public record GameRecordQueryViewModel(
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
