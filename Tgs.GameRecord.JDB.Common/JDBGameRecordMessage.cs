using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.JDB.Common;

public record JDBGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "http://api.jygrq.com/")]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "ZF")]
	public string Domain { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "193a3159afebee65")]
	public string AesKey { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "831057181dd07807")]
	public string AesIV { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "zf055gpi")]
	public string Agent { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-03-13T15:45:00+08:00")]
	public DateTimeOffset StartTime { get; init; }
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-03-13T16:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; }
}
