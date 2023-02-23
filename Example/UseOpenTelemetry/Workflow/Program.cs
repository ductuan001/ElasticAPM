using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Elastic.Apm.NetCoreAll;
using OpenTelemetry.Metrics;
using Workflow.Constant;


var builder = WebApplication.CreateBuilder(args);;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
    {
        b.AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .ConfigureResource(r =>
            {
                r.AddService(
                    serviceName: TelemetryConstants.ServiceName,
                    serviceVersion: TelemetryConstants.ServiceVersion,
                    serviceInstanceId: Environment.MachineName
                );
            })
            .AddSource(TelemetryConstants.ServiceName)
            .AddOtlpExporter(opt =>
            {
                opt.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();