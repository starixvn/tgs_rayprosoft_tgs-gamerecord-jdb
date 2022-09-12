namespace Tgs.GameRecord.JDBService;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
