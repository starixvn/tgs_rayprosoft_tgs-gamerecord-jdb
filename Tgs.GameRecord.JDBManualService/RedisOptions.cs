namespace Tgs.GameRecord.JDBManualService;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
