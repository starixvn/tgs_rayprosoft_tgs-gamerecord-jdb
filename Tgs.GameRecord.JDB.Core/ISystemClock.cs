namespace Tgs.GameRecord.JDB.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}
