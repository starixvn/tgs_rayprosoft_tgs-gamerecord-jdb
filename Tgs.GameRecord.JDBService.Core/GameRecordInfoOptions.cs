namespace Tgs.GameRecord.JDBService.Core;

public record GameRecordInfoOptions()
{
    public string MongoDefine { get; init; } = default!;
    public string MongoGlobPattern { get; init; } = default!;
    public string GameProviderCode { get; set; } = default!;
    public double MaxIntervalMinute { get; set; } = 10d;
    public double MinIntervalMinute { get; set; } = 1d;

    public double IntervalMinute { get; set; } = 10d;
    public double LockMinute { get; set; } = 8.5d;
}
