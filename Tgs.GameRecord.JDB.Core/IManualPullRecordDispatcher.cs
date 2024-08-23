using Tgs.GameRecord.JDB.Core.ApplicationServices;

namespace Tgs.GameRecord.JDB.Core;

public interface IManualPullRecordDispatcher
{
	ValueTask PushCommandAsync(ManualPullRecordCommand command, CancellationToken cancellationToken = default);
}
