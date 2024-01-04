using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameRecord.JDB.Common;

public record JDBGameRecordMessage
{
    [Required]
    [DataType(DataType.Url)]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string Domain { get; init; } = default!;
    [Required]
    public string AesKey { get; init; } = default!;
    [Required]
    public string AesIV { get; init; } = default!;
    [Required]
    public string Agent { get; init; } = default!;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartTime { get; init; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset EndTime { get; init; }
}
