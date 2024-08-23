namespace Tgs.GameRecord.JDB.Core;

internal class UtensilRepository(
	TimeProvider timeProvider) : IUtensil
{
	public bool TimeDifferenceWithin2Hours(DateTimeOffset source)
	{
		var hours = timeProvider.GetUtcNow().Subtract(source).TotalHours;

		return hours is > 0 and <= 2;
	}
}
