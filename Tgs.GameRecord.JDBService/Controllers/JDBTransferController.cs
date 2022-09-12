using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.JDBService.Core.WorkerServices;
using Tgs.GameRecord.JDBService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.JDBService.Controllers;

[ApiController]
[Route("jdbtf")]
public class JDBTransferController : ControllerBase
{
    /// <summary>
    /// Executes the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl": "http://api.jygrq.com",
    ///             "domain": "ZF",
    ///             "aesKey": "831057181dd07807",
    ///             "aesIV": "193a3159afebee65",
    ///             "agent": "zf055gpi",
    ///             "startTime": "2022-09-07T18:50:00+08:00",
    ///             "endTime": "2022-09-07T18:55:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost]
    public ValueTask ExecuteAsync(
        [FromServices] ICommandService<ManualWorkerCommand> service,
        GameRecordQueryViewModel model)
        => service.ExecuteAsync(new ManualWorkerCommand(
            ApiUrl: model.ApiUrl,
            Domain: model.Domain,
            AesKey: model.AesKey,
            AesIV: model.AesIV,
            Agent: model.Agent,
            StartTime: model.StartTime.UtcDateTime,
            EndTime: model.EndTime.UtcDateTime));
}
