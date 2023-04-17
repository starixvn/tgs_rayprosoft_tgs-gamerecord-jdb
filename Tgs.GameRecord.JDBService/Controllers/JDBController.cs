using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.GameRecord.JDBService.Core;
using Tgs.GameRounds.Schedule.Client;
using Tgs.GameRounds.Schedule.Protos;
using Tgs.GameRecord.JDBService.ViewModels;

namespace Tgs.GameRecord.JDBService.Controllers;

[Route("[controller]")]
[ApiController]
public class JDBController : ControllerBase
{
    private readonly GameRecordInfoOptions _gameRecordInfo;

    public JDBController(
        IOptions<GameRecordInfoOptions> options)
    {
        _gameRecordInfo = options.Value; ;
    }

    /// <summary>
    /// Manuals the game record asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl":"http://api.jygrq.com/",
    ///             "domain":"ZF",
    ///             "aesKey":"193a3159afebee65",
    ///             "aesIV":"831057181dd07807",
    ///             "agent":"zf055gpi",
    ///             "startTime": "2023-03-13T15:45:00+08:00",
    ///             "endTime": "2023-03-13T16:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("manual")]
    public ValueTask ManualGameRecordAsync(
        [FromServices] IGameRoundsScheduleClient client,
        JDBViewModel source)
        => client.JDB_ScheduleFormQueuedAsync(
             gameProviderCode: _gameRecordInfo.GameProviderCode,
             name: source.Agent,
             subject: "gp.gamerecord.scheduleform.jdbtf.queued",
             source: ScheduleFormSource.Manual,
             apiUrl: source.ApiUrl,
             domain: source.Domain,
             aesKey: source.AesKey,
             aesIV: source.AesIV,
             agent: source.Agent,
             startTime: source.StartTime,
             endTime: source.EndTime,
             cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Schedules the game record asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl":"http://api.jygrq.com/",
    ///             "domain":"ZF",
    ///             "aesKey":"193a3159afebee65",
    ///             "aesIV":"831057181dd07807",
    ///             "agent":"zf055gpi",
    ///             "startTime": "2023-03-13T15:45:00+08:00",
    ///             "endTime": "2023-03-13T16:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("schedule")]
    public ValueTask ScheduleGameRecordAsync(
        [FromServices] IGameRoundsScheduleClient client,
        JDBViewModel source)
        => client.JDB_ScheduleFormQueuedAsync(
             gameProviderCode: _gameRecordInfo.GameProviderCode,
             name: source.Agent,
             subject: "gp.gamerecord.scheduleform.jdbtf.queued",
             source: ScheduleFormSource.Manual,
             apiUrl: source.ApiUrl,
             domain: source.Domain,
             aesKey: source.AesKey,
             aesIV: source.AesIV,
             agent: source.Agent,
             startTime: source.StartTime,
             endTime: source.EndTime,
             cancellationToken: HttpContext.RequestAborted);
}
