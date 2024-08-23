using Tgs.GameRecord.JDB.Core.ApplicationServices;

namespace Tgs.GameRecord.JDB.Core;

public interface ISchedulePullRecordDispatcher
{
	ValueTask PushCommandAsync(SchedulePullRecordCommand command, CancellationToken cancellationToken = default);
}
