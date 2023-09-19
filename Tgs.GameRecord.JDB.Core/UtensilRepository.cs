namespace Tgs.GameRecord.JDB.Core;

internal class UtensilRepository : IUtensil
{
    private readonly ISystemClock _systemClock;

    public UtensilRepository(
        ISystemClock systemClock)
    {
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public bool TimeDifferenceWithin2Hours(DateTimeOffset source)
    {
        var hours = _systemClock.CurrentTime.Subtract(source).TotalHours;

        return hours is > 0 and <= 2;
    }
}
