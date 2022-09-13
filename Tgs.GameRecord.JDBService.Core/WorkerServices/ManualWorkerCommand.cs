namespace Tgs.GameRecord.JDBService.Core.WorkerServices;

public record ManualWorkerCommand(
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent,
    DateTime StartTime,
    DateTime EndTime);
