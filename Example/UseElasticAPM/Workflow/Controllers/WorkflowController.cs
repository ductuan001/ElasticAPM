using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace Workflow.Controllers;

[ApiController]
[Route("api")]
public class WorkflowController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WorkflowController> _logger;
    private HttpClientHandler clientHandler = new HttpClientHandler();
    public WorkflowController(ILogger<WorkflowController> logger)
    {
        _logger = logger;
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
    }

    [HttpGet("workflowAPIHook")]
    public async Task<String> workflowAPIHook()
    {
        var response = "Received request";
        try
        {
            sendMessageToWorker();
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not send request", ex);
            response = "Receive failed, please call again";
        }

        return response;
    }

    public async Task sendMessageToWorker()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7005/api/sendRequestToWorker");
        var response = await new HttpClient(clientHandler).SendAsync(request);
        var result = response.Content.ReadAsStringAsync().Result;
        callBackToOrder(result);
    }

    public async Task callBackToOrder(String message)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7292/api/callback/" + message);
        var responseMessage = new HttpClient(clientHandler).Send(request);
        Console.WriteLine("test");
    }
}