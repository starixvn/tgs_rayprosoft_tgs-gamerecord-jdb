using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.JDBService.ViewModels;

public record JDBViewModel
{
    [Required]
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
    public DateTime StartTime { get; init; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndTime { get; init; }
}
