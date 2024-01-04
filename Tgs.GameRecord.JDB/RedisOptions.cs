using Tgs.GameRecord.JDB.Core;
namespace Tgs.GameRecord.JDB;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
