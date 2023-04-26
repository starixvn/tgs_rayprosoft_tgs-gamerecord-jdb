namespace Tgs.GameRecord.JDBService.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}
