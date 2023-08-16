namespace Tgs.GameRecord.JDBService.Core;

public interface IUtensil
{
    bool TimeDifferenceWithin2Hours(DateTimeOffset source);
}
