using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using Order.Constant;

namespace Order.Controllers;

[ApiController]
[Route("api")]
public class OrderAPIController : ControllerBase
{
    // TelemetryClient
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<OrderAPIController> _logger;
    private HttpClientHandler clientHandler = new HttpClientHandler();
    public OrderAPIController(ILogger<OrderAPIController> logger)
    {
        _logger = logger;
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
    }
    [HttpGet("sendRequestToWorkflow")]
    public String sendRequestToWorkflow()
    {
        
        var result = "";
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7245/api/workflowAPIHook");
            var response =  new HttpClient(clientHandler).Send(request);
            result = response.Content.ReadAsStringAsync().Result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not send request", ex);
            result = "Could not send request, please try again";
        }

        return result;
    }
    
    [HttpGet("callback/{message}")]
    public void ReceiveMessageFromQueue(String message)
    {
        Console.WriteLine(message);
    }
}