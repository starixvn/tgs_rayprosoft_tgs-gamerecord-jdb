namespace Tgs.GameRecord.JDB.Core;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
