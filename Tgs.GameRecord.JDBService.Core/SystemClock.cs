namespace Tgs.GameRecord.JDBService.Core;

internal class SystemClock : ISystemClock
{
    public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}
