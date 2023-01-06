namespace Tgs.GameRecord.JDBService.Core;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
