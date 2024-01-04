using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.JDB.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.JDB.Controllers;

[Route("[controller]")]
[ApiController]
public class JDBController : ControllerBase
{
    /// <summary>
    /// Games the record manual asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
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
    public ValueTask GameRecordManualAsync(
        [FromServices] IMessageSender messageSender,
        JDBGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: JDBNatsConst.ManualPublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the record schedule asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
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
    public ValueTask GameRecordScheduleAsync(
        [FromServices] IMessageSender messageSender,
        JDBGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: JDBNatsConst.SchedulePublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);
}
