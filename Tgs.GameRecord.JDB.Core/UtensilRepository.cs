namespace Tgs.GameRecord.JDB.Core;

internal class UtensilRepository(
	ISystemClock systemClock) : IUtensil
{
	public bool TimeDifferenceWithin2Hours(DateTimeOffset source)
	{
		var hours = systemClock.CurrentTime.Subtract(source).TotalHours;

		return hours is > 0 and <= 2;
	}
}
