using Tgs.GameRecord.JDB.Core.JobApplicationServices;

namespace Tgs.GameRecord.JDB.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
