namespace Tgs.GameRecord.JDB.Core.ApplicationServices;

public record SchedulePullRecordCommand(
	Uri ApiUrl,
	string Domain,
	string AesKey,
	string AesIV,
	string Agent,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
