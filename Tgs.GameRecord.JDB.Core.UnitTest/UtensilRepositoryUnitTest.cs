using NSubstitute;

namespace Tgs.GameRecord.JDB.Core.UnitTest;

public class UtensilRepositoryUnitTest
{
    [Fact]
    public void Time_Difference_Within_2_Hours_Less_Than_10_Seconds_True()
    {
        var fakeSystemClock = Substitute.For<ISystemClock>();

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            fakeSystemClock);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T12:59:50+08:00"));

        Assert.True(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_Less_Than_2_Hours_True()
    {
        var fakeSystemClock = Substitute.For<ISystemClock>();

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            fakeSystemClock);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T11:00:00+08:00"));

        Assert.True(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_Less_Than_20_Minutes_True()
    {
        var fakeSystemClock = Substitute.For<ISystemClock>();

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            fakeSystemClock);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T12:40:00+08:00"));

        Assert.True(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_More_Than_10_Seconds_False()
    {
        var fakeSystemClock = Substitute.For<ISystemClock>();

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            fakeSystemClock);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T13:00:10+08:00"));

        Assert.False(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_More_Than_2_Hours_False()
    {
        var fakeSystemClock = Substitute.For<ISystemClock>();

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            fakeSystemClock);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T15:00:00+08:00"));

        Assert.False(actual);
    }

    [Fact]
    public void Time_Difference_Within_2_Hours_More_Than_20_Minutes_False()
    {
        var fakeSystemClock = Substitute.For<ISystemClock>();

        _ = fakeSystemClock.CurrentTime.Returns(DateTimeOffset.Parse("2023-04-13T13:00:00+08:00"));

        var repository = new UtensilRepository(
            fakeSystemClock);

        var actual = repository.TimeDifferenceWithin2Hours(source: DateTimeOffset.Parse("2023-04-13T13:20:00+08:00"));

        Assert.False(actual);
    }
}
