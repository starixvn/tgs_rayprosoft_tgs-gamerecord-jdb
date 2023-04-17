namespace Tgs.GameRecord.JDBService.Core;

public record GameRecordInfoOptions()
{
    public string GameProviderCode { get; set; } = default!;
}
