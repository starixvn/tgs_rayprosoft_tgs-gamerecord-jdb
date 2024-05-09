namespace Tgs.GameRecord.JDB.Core;

public interface IUtensil
{
	bool TimeDifferenceWithin2Hours(DateTimeOffset source);
}
