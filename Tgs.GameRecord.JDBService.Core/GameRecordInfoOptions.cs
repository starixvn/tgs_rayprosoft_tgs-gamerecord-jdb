namespace Tgs.GameRecord.JDBService.Core;

public record GameRecordInfoOptions()
{
    public string GameProviderCode { get; set; } = default!;
    public double MaximumIntervalMinute { get; set; } = 10d;
    public double MinimumIntervalMinute { get; set; } = 1d;
    public double IntervalMinute { get; set; } = 10d;
    public double LockMinute { get; set; } = 8.5d;
}
