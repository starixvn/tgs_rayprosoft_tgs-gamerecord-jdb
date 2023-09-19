namespace Tgs.GameRecord.JDB.Core.JobApplicationServices;

public record JobExecuteCommand(
    string Name,
    string GameProviderCode,
    Uri ApiUrl,
    string Domain,
    string AesKey,
    string AesIV,
    string Agent);
