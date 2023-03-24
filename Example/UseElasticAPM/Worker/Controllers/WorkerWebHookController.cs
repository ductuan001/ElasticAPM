using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Worker.Controllers;

[ApiController]
[Route("api")]
public class WorkerWebHookController : ControllerBase
{

    private readonly ILogger<WorkerWebHookController> _logger;
    public WorkerWebHookController(ILogger<WorkerWebHookController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("sendRequestToWorker")]
    public String WorkerReceiveRequest()
    {
        _logger.LogInformation("Worker completed task");
        return "Worker completed task";
    }
}